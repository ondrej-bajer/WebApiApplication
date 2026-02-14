using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiApplication.Services;
using Xunit;

namespace WebApiTest;

public class ProductServiceTests
{
    [Fact]
    public void GetAll_ReturnsAllProducts()
    {
        var svc = new InMemoryProductService();

        var products = svc.GetAll();

        Assert.NotEmpty(products);
        Assert.Equal(2, products.Count);
    }

    [Fact]
    public void GetById_ReturnsNull_WhenProductDoesNotExist()
    {
        var svc = new InMemoryProductService();

        var result = svc.GetById(999);

        Assert.Null(result);
    }

    [Fact]
    public void UpdateDescription_ReturnsFalse_WhenProductDoesNotExist()
    {
        var svc = new InMemoryProductService();

        var ok = svc.UpdateDescription(999, "x");

        Assert.False(ok);
    }

    [Fact]
    public void UpdateDescription_WhenProductExists()
    {
        var svc = new InMemoryProductService();

        var ok = svc.UpdateDescription(1, "updated");

        Assert.True(ok);
        Assert.Equal("updated", svc.GetById(1)!.Description);
    }
}
