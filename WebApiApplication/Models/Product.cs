using System.ComponentModel.DataAnnotations;

namespace WebApiApplication.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string ImgUri { get; set; } = string.Empty;

        [Required]
        public decimal Price { get; set; }
        
        public string? Description { get; set; }
    }
}
