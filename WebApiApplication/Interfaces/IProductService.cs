using WebApiApplication.DTOs;

namespace WebApiApplication.Interfaces
{
    public interface IProductService
    {
        IReadOnlyList<ProductDto> GetAll();
        PagedResponse<ProductDto> GetPaged(int page, int pageSize);
        ProductDto? GetById(int id);
        bool UpdateDescription(int id, string? description);
    }
}
