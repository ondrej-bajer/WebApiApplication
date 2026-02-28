using WebApiApplication.DTOs;

namespace WebApiApplication.Interfaces
{
    public interface IProductService
    {
        //IReadOnlyList<ProductDto> GetAll();
        //PagedResponse<ProductDto> GetPaged(int page, int pageSize);
        //ProductDto? GetById(int id);
        //bool UpdateDescription(int id, string? description);

        Task<IReadOnlyList<ProductDto>> GetAllAsync(CancellationToken ct = default);
        Task<ProductDto?> GetByIdAsync(int id, CancellationToken ct = default);
        Task UpdateDescriptionAsync(int id, string? description, CancellationToken ct = default);

        Task<PagedResponse<ProductDto>> GetPagedAsync(int page, int pageSize, CancellationToken ct = default);
    }
}
