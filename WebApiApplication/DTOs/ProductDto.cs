namespace WebApiApplication.DTOs
{
    public record ProductDto(
        int Id,
        string Name,
        string ImgUri,
        decimal Price,
        string? Description);
}
