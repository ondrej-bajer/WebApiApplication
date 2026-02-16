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
    public async Task UpdateDescriptionAsync_ReturnsFalse_WhenProductDoesNotExist()
    {
        IProductService svc = new InMemoryProductService();

        var ok = await svc.UpdateDescriptionAsync(999, "x");

        Assert.False(ok);
    }

    [Fact]
    public async Task UpdateDescriptionAsync_WhenProductExists()
    {
        IProductService svc = new InMemoryProductService();

        var ok = await svc.UpdateDescriptionAsync(1, "updated");

        Assert.True(ok);

        var product = await svc.GetByIdAsync(1);
        Assert.NotNull(product);
        Assert.Equal("updated", product!.Description);
    }
}
