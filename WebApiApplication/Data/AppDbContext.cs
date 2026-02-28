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
                .HasMaxLength(2000)
                .IsRequired(false);

            // Seed
            product.HasData(
                new Product { Id = 1, Name = "Laptop", ImgUri = "https://img/1", Price = 1000m, Description = "Gaming laptop" },
                new Product { Id = 2, Name = "Mouse", ImgUri = "https://img/2", Price = 50m, Description = null },
                new Product { Id = 3, Name = "Keyboard", ImgUri = "https://img/3", Price = 120m, Description = "Mechanical keyboard" },
                new Product { Id = 4, Name = "Monitor 27\"", ImgUri = "https://img/4", Price = 280m, Description = "IPS, 1440p" },
                new Product { Id = 5, Name = "Monitor 34\" UW", ImgUri = "https://img/5", Price = 520m, Description = "Ultrawide, 144Hz" },
                new Product { Id = 6, Name = "USB-C Hub", ImgUri = "https://img/6", Price = 45m, Description = "HDMI + USB-A + PD" },
                new Product { Id = 7, Name = "Webcam", ImgUri = "https://img/7", Price = 80m, Description = "1080p webcam" },
                new Product { Id = 8, Name = "Headset", ImgUri = "https://img/8", Price = 90m, Description = "Closed-back headset" },
                new Product { Id = 9, Name = "Microphone", ImgUri = "https://img/9", Price = 110m, Description = "USB condenser mic" },
                new Product { Id = 10, Name = "Speakers", ImgUri = "https://img/10", Price = 65m, Description = "2.0 desktop speakers" },
                new Product { Id = 11, Name = "External SSD 1TB", ImgUri = "https://img/11", Price = 140m, Description = "USB 3.2 Gen2" },
                new Product { Id = 12, Name = "External HDD 2TB", ImgUri = "https://img/12", Price = 85m, Description = "Portable storage" },
                new Product { Id = 13, Name = "Wi-Fi Router", ImgUri = "https://img/13", Price = 150m, Description = "Wi-Fi 6 router" },
                new Product { Id = 14, Name = "Ethernet Switch", ImgUri = "https://img/14", Price = 55m, Description = "8-port gigabit" },
                new Product { Id = 15, Name = "Smart Plug", ImgUri = "https://img/15", Price = 18m, Description = "Energy monitoring" },
                new Product { Id = 16, Name = "Smart Bulb", ImgUri = "https://img/16", Price = 15m, Description = "RGB + warm white" },
                new Product { Id = 17, Name = "Desk Lamp", ImgUri = "https://img/17", Price = 35m, Description = "Adjustable brightness" },
                new Product { Id = 18, Name = "Laptop Stand", ImgUri = "https://img/18", Price = 30m, Description = "Aluminum stand" },
                new Product { Id = 19, Name = "Office Chair Mat", ImgUri = "https://img/19", Price = 25m, Description = null },
                new Product { Id = 20, Name = "Cable Organizer", ImgUri = "https://img/20", Price = 12m, Description = "Under-desk tray" },
                new Product { Id = 21, Name = "Power Strip", ImgUri = "https://img/21", Price = 22m, Description = "Surge protection" },
                new Product { Id = 22, Name = "UPS 650VA", ImgUri = "https://img/22", Price = 110m, Description = "Backup power" },
                new Product { Id = 23, Name = "HDMI Cable 2m", ImgUri = "https://img/23", Price = 10m, Description = "High speed HDMI" },
                new Product { Id = 24, Name = "USB-C Cable 1m", ImgUri = "https://img/24", Price = 9m, Description = "100W PD cable" },
                new Product { Id = 25, Name = "Mouse Pad XL", ImgUri = "https://img/25", Price = 14m, Description = "Extended desk mat" },
                new Product { Id = 26, Name = "Portable Charger", ImgUri = "https://img/26", Price = 40m, Description = "10,000mAh" },
                new Product { Id = 27, Name = "Bluetooth Adapter", ImgUri = "https://img/27", Price = 13m, Description = "USB Bluetooth 5.0" },
                new Product { Id = 28, Name = "USB Flash 128GB", ImgUri = "https://img/28", Price = 16m, Description = null },
                new Product { Id = 29, Name = "Printer", ImgUri = "https://img/29", Price = 130m, Description = "Mono laser printer" },
                new Product { Id = 30, Name = "Scanner", ImgUri = "https://img/30", Price = 95m, Description = "Document scanner" }
            );
        }
    }
}
