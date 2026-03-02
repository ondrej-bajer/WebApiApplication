using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiApplication.Interfaces;
using WebApiApplication.Services;
using Xunit;

namespace WebApiTest;

public class ProductServiceTests
{
    [Fact]
    public async Task GetAllAsync_ReturnsAllProducts()
    {
        IProductService svc = new InMemoryProductService();

        var products = await svc.GetAllAsync();

        Assert.NotEmpty(products);
        Assert.Equal(30, products.Count);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenProductDoesNotExist()
    {
        IProductService svc = new InMemoryProductService();

        var result = await svc.GetByIdAsync(999);

        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateDescriptionAsync_WhenProductDoesNotExist_Throws()
    {
        IProductService svc = new InMemoryProductService();

        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            svc.UpdateDescriptionAsync(999, "x"));
    }

    [Fact]
    public async Task UpdateDescriptionAsync_WhenProductExists_UpdatesValue()
    {
        IProductService svc = new InMemoryProductService();

        await svc.UpdateDescriptionAsync(1, "updated");

        var product = await svc.GetByIdAsync(1);
        Assert.NotNull(product);
        Assert.Equal("updated", product!.Description);
    }

    [Fact]
    public async Task UpdateDescriptionAsync_IsIdempotent()
    {
        IProductService svc = new InMemoryProductService();

        await svc.UpdateDescriptionAsync(1, "same");
        var first = await svc.GetByIdAsync(1);

        await svc.UpdateDescriptionAsync(1, "same");
        var second = await svc.GetByIdAsync(1);

        Assert.Equal(first!.Description, second!.Description);
    }
}
