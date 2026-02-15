using System.Net;
using System.Net.Http.Json;
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

    [Fact]
    public async Task GetAll_v1_returns_200_and_list()
    {
        var response = await _client.GetAsync("/api/v1/products");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var items = await response.Content.ReadFromJsonAsync<List<ProductDto>>();
        Assert.NotNull(items);
        Assert.NotEmpty(items!);
    }

    [Fact]
    public async Task GetById_v1_existing_returns_200()
    {
        var response = await _client.GetAsync("/api/v1/products/1");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var item = await response.Content.ReadFromJsonAsync<ProductDto>();
        Assert.NotNull(item);
        Assert.Equal(1, item!.Id);
    }

    [Fact]
    public async Task GetById_v1_not_found_returns_404()
    {
        var response = await _client.GetAsync("/api/v1/products/999999");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetById_v1_invalid_id_returns_400()
    {
        var response = await _client.GetAsync("/api/v1/products/0");
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task PatchDescription_v1_existing_returns_204_and_updates_value()
    {
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
    public async Task PatchDescription_v1_not_found_returns_404()
    {
        var patchBody = new UpdateProductDescriptionRequest { Description = "Does not matter" };

        var response = await _client.PatchAsJsonAsync("/api/v1/products/999999/description", patchBody);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetAll_v2_default_pagination_returns_200_and_pageSize_10()
    {
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
    public async Task GetAll_v2_custom_pagination_returns_expected_size()
    {
        var response = await _client.GetAsync("/api/v2/products?page=2&pageSize=5");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var page = await response.Content.ReadFromJsonAsync<PagedResponse<ProductDto>>();
        Assert.NotNull(page);

        Assert.Equal(2, page!.Page);
        Assert.Equal(5, page.PageSize);
        Assert.True(page.Items.Count <= 5);
    }

    [Fact]
    public async Task GetAll_v2_invalid_page_returns_400_if_model_validation_enabled()
    {
        // This test is valid for PaginationQuery [Range(1, ...)].
        var response = await _client.GetAsync("/api/v2/products?page=0&pageSize=10");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
