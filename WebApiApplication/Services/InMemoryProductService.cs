using WebApiApplication.DTOs;
using WebApiApplication.Interfaces;
using WebApiApplication.Models;

namespace WebApiApplication.Services
{
    public class InMemoryProductService : IProductService
    {
        private readonly List<Product> _products =
            [
                new() { Id = 1, Name = "Laptop", ImgUri = "https://img/1", Price = 1000m, Description = "Gaming laptop" },
                new() { Id = 2, Name = "Mouse", ImgUri = "https://img/2", Price = 50m, Description = null },
            ];

        public IReadOnlyList<ProductDto> GetAll()
            => _products.Select(ToDto).ToList();

        public ProductDto? GetById(int id)
            => _products.Where(p => p.Id == id).Select(ToDto).FirstOrDefault();

        public bool UpdateDescription(int id, string? description)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product is null) return false;

            product.Description = description;
            return true;
        }

        private static ProductDto ToDto(Product p)
            => new(p.Id, p.Name, p.ImgUri, p.Price, p.Description);
    }
}
