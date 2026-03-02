using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using WebApiApplication.DTOs;
using Xunit;

namespace WebApiTest;

public class ProductsEndpointsTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public ProductsEndpointsTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    // -----------------------
    // Helpers
    // -----------------------

    private async Task<string> LoginAndGetTokenAsync(string username, string password)
    {
        var resp = await _client.PostAsJsonAsync("/api/auth/login", new { username, password });
        resp.EnsureSuccessStatusCode();

        using var doc = JsonDocument.Parse(await resp.Content.ReadAsStringAsync());
        return doc.RootElement.GetProperty("accessToken").GetString()
               ?? throw new InvalidOperationException("Token missing in response.");
    }

    private async Task AuthorizeAsAdminAsync()
    {
        var token = await LoginAndGetTokenAsync("admin", "admin123");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    private async Task AuthorizeAsUserAsync()
    {
        var token = await LoginAndGetTokenAsync("user", "user123");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    private void ClearAuthorization()
    {
        _client.DefaultRequestHeaders.Authorization = null;
    }

    // -----------------------
    // Auth behavior
    // -----------------------

    [Fact]
    public async Task GetAll_v1_without_token_returns_401()
    {
        ClearAuthorization();

        var response = await _client.GetAsync("/api/v1/products");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    // -----------------------
    // v1 - Authorized reads
    // -----------------------

    [Fact]
    public async Task GetAll_v1_returns_200_and_list_when_authorized()
    {
        await AuthorizeAsAdminAsync();

        var response = await _client.GetAsync("/api/v1/products");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var items = await response.Content.ReadFromJsonAsync<List<ProductDto>>();
        Assert.NotNull(items);
        Assert.NotEmpty(items!);
    }

    [Fact]
    public async Task GetById_v1_existing_returns_200_when_authorized()
    {
        await AuthorizeAsAdminAsync();

        var response = await _client.GetAsync("/api/v1/products/1");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var item = await response.Content.ReadFromJsonAsync<ProductDto>();
        Assert.NotNull(item);
        Assert.Equal(1, item!.Id);
    }

    [Fact]
    public async Task GetById_v1_not_found_returns_404_when_authorized()
    {
        await AuthorizeAsAdminAsync();

        var response = await _client.GetAsync("/api/v1/products/999999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetById_v1_invalid_id_returns_400_when_authorized()
    {
        await AuthorizeAsAdminAsync();

        var response = await _client.GetAsync("/api/v1/products/0");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    // -----------------------
    // v1 - PATCH (Admin only)
    // -----------------------

    [Fact]
    public async Task PatchDescription_v1_as_user_returns_403()
    {
        await AuthorizeAsUserAsync();

        var patchBody = new UpdateProductDescriptionRequest { Description = "Updated description" };

        var response = await _client.PatchAsJsonAsync("/api/v1/products/1/description", patchBody);

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task PatchDescription_v1_existing_as_admin_returns_204_and_updates_value()
    {
        await AuthorizeAsAdminAsync();

        // arrange
        var patchBody = new UpdateProductDescriptionRequest { Description = "Updated description" };

        // act
        var patchResponse = await _client.PatchAsJsonAsync("/api/v1/products/1/description", patchBody);

        // assert
        Assert.Equal(HttpStatusCode.NoContent, patchResponse.StatusCode);

        // verify by reading the product
        var getResponse = await _client.GetAsync("/api/v1/products/1");
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

        var item = await getResponse.Content.ReadFromJsonAsync<ProductDto>();
        Assert.NotNull(item);
        Assert.Equal("Updated description", item!.Description);
    }

    [Fact]
    public async Task PatchDescription_v1_not_found_returns_404_when_authorized_as_admin()
    {
        await AuthorizeAsAdminAsync();

        var patchBody = new UpdateProductDescriptionRequest { Description = "Does not matter" };

        var response = await _client.PatchAsJsonAsync("/api/v1/products/999999/description", patchBody);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    // -----------------------
    // v2 - Pagination (Authorized)
    // -----------------------

    [Fact]
    public async Task GetAll_v2_default_pagination_returns_200_and_pageSize_10_when_authorized()
    {
        await AuthorizeAsAdminAsync();

        var response = await _client.GetAsync("/api/v2/products");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var page = await response.Content.ReadFromJsonAsync<PagedResponse<ProductDto>>();
        Assert.NotNull(page);

        Assert.Equal(1, page!.Page);
        Assert.Equal(10, page.PageSize);
        Assert.True(page.TotalCount > 0);
        Assert.True(page.Items.Count <= 10);
    }

    [Fact]
    public async Task GetAll_v2_custom_pagination_returns_expected_size_when_authorized()
    {
        await AuthorizeAsAdminAsync();

        var response = await _client.GetAsync("/api/v2/products?page=2&pageSize=5");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var page = await response.Content.ReadFromJsonAsync<PagedResponse<ProductDto>>();
        Assert.NotNull(page);

        Assert.Equal(2, page!.Page);
        Assert.Equal(5, page.PageSize);
        Assert.True(page.Items.Count <= 5);
    }

    [Fact]
    public async Task GetAll_v2_invalid_page_returns_400_when_authorized()
    {
        await AuthorizeAsAdminAsync();

        // This test is valid for PaginationQuery [Range(1, ...)].
        var response = await _client.GetAsync("/api/v2/products?page=0&pageSize=10");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}