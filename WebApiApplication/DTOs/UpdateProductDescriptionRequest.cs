using System.ComponentModel.DataAnnotations;

namespace WebApiApplication.DTOs
{
    public class UpdateProductDescriptionRequest
    {
        [Required]
        [MinLength(1)]
        [MaxLength(2000)]
        public string Description { get; init; } = string.Empty;
    }
}
