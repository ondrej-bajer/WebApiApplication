using WebApiApplication.DTOs;

namespace WebApiApplication.Interfaces
{
    public interface IProductService
    {

        Task<IReadOnlyList<ProductDto>> GetAllAsync(CancellationToken ct = default);
        Task<ProductDto?> GetByIdAsync(int id, CancellationToken ct = default);
        Task UpdateDescriptionAsync(int id, string? description, CancellationToken ct = default);

        Task<PagedResponse<ProductDto>> GetPagedAsync(int page, int pageSize, CancellationToken ct = default);
    }
}
