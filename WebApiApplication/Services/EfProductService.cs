using Microsoft.EntityFrameworkCore;
using WebApiApplication.Data;
using WebApiApplication.DTOs;
using WebApiApplication.Interfaces;

namespace WebApiApplication.Services
{
    public class EfProductService : IProductService
    {
        private readonly AppDbContext _db;

        public EfProductService(AppDbContext db) => _db = db;

        public IReadOnlyList<ProductDto> GetAll()
            => _db.Products.AsNoTracking()
                .Select(p => new ProductDto(p.Id, p.Name, p.ImgUri, p.Price, p.Description))
                .ToList();

        public ProductDto? GetById(int id)
            => _db.Products.AsNoTracking()
                .Where(p => p.Id == id)
                .Select(p => new ProductDto(p.Id, p.Name, p.ImgUri, p.Price, p.Description))
                .FirstOrDefault();

        public bool UpdateDescription(int id, string? description)
        {
            var product = _db.Products.FirstOrDefault(p => p.Id == id);
            if (product is null) return false;

            product.Description = description;
            _db.SaveChanges();
            return true;
        }

        public PagedResponse<ProductDto> GetPaged(int page, int pageSize)
        {
            page = page < 1 ? 1 : page;
            pageSize = pageSize < 1 ? 10 : pageSize;
            pageSize = pageSize > 100 ? 100 : pageSize;

            var query = _db.Products.AsNoTracking();

            var totalCount = query.Count();

            var items = query
                .OrderBy(p => p.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new ProductDto(
                    p.Id,
                    p.Name,
                    p.ImgUri,
                    p.Price,
                    p.Description))
                .ToList();

            return new PagedResponse<ProductDto>(items, page, pageSize, totalCount);
        }
    }
}
