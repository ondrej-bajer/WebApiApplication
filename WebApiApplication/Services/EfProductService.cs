using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using WebApiApplication.Data;
using WebApiApplication.DTOs;
using WebApiApplication.Interfaces;

namespace WebApiApplication.Services
{
    public class EfProductService : IProductService
    {
        private readonly AppDbContext _db;

        public EfProductService(AppDbContext db) => _db = db;

        public async Task<IReadOnlyList<ProductDto>> GetAllAsync(CancellationToken ct = default)
            => await _db.Products.AsNoTracking()
                .Select(p => new ProductDto(p.Id, p.Name, p.ImgUri, p.Price, p.Description))
                .ToListAsync(ct);

        public async Task<ProductDto?> GetByIdAsync(int id, CancellationToken ct = default)
            => await _db.Products.AsNoTracking()
                .Where(p => p.Id == id)
                .Select(p => new ProductDto(p.Id, p.Name, p.ImgUri, p.Price, p.Description))
                .FirstOrDefaultAsync(ct);

        public async Task UpdateDescriptionAsync(int id, string? description, CancellationToken ct = default)
        {
            if (id <= 0)
                throw new ArgumentException("Id must be a positive integer.", nameof(id));

            var product = await _db.Products.FirstOrDefaultAsync(p => p.Id == id, ct);
            if (product is null)
                throw new KeyNotFoundException($"Product with id {id} was not found.");

            // Normalizace whitespace, ale null zůstává null
            product.Description = description?.Trim();

            await _db.SaveChangesAsync(ct);
        }

        public async Task<PagedResponse<ProductDto>> GetPagedAsync(int page, int pageSize, CancellationToken ct = default)
        {
            page = page < 1 ? 1 : page;
            pageSize = pageSize < 1 ? 10 : pageSize;
            pageSize = pageSize > 100 ? 100 : pageSize;

            var query = _db.Products.AsNoTracking();

            var totalCount = await query.CountAsync(ct);

            var items = await query
                .OrderBy(p => p.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new ProductDto(p.Id, p.Name, p.ImgUri, p.Price, p.Description))
                .ToListAsync(ct);

            return new PagedResponse<ProductDto>(items, page, pageSize, totalCount);
        }
    }
}
