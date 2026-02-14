using Microsoft.EntityFrameworkCore;
using WebApiApplication.Models;

namespace WebApiApplication.Data

{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products => Set<Product>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var product = modelBuilder.Entity<Product>();

            product.HasKey(p => p.Id);

            product.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(200);

            product.Property(p => p.ImgUri)
                .IsRequired()
                .HasMaxLength(2048);

            product.Property(p => p.Price)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            product.Property(p => p.Description)
                .IsRequired(false);

            // Seed
            product.HasData(
                new Product { Id = 1, Name = "Laptop", ImgUri = "https://img/1", Price = 1000m, Description = "Gaming laptop" },
                new Product { Id = 2, Name = "Mouse", ImgUri = "https://img/2", Price = 50m, Description = null }
            );
        }
    }
}
