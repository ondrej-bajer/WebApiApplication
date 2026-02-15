using WebApiApplication.DTOs;
using WebApiApplication.Interfaces;
using WebApiApplication.Models;

namespace WebApiApplication.Services
{
    public class InMemoryProductService : IProductService
    {
        private readonly List<Product> _products =
            [
                new() { Id = 1,  Name = "Laptop",            ImgUri = "https://img/1",  Price = 1000m, Description = "Gaming laptop" },
                new() { Id = 2,  Name = "Mouse",             ImgUri = "https://img/2",  Price = 50m,   Description = null },
                new() { Id = 3,  Name = "Keyboard",          ImgUri = "https://img/3",  Price = 120m,  Description = "Mechanical keyboard" },
                new() { Id = 4,  Name = "Monitor 27\"",      ImgUri = "https://img/4",  Price = 280m,  Description = "IPS, 1440p" },
                new() { Id = 5,  Name = "Monitor 34\" UW",   ImgUri = "https://img/5",  Price = 520m,  Description = "Ultrawide, 144Hz" },
                new() { Id = 6,  Name = "USB-C Hub",         ImgUri = "https://img/6",  Price = 45m,   Description = "HDMI + USB-A + PD" },
                new() { Id = 7,  Name = "Webcam",            ImgUri = "https://img/7",  Price = 80m,   Description = "1080p webcam" },
                new() { Id = 8,  Name = "Headset",           ImgUri = "https://img/8",  Price = 90m,   Description = "Closed-back headset" },
                new() { Id = 9,  Name = "Microphone",        ImgUri = "https://img/9",  Price = 110m,  Description = "USB condenser mic" },
                new() { Id = 10, Name = "Speakers",          ImgUri = "https://img/10", Price = 65m,   Description = "2.0 desktop speakers" },

                new() { Id = 11, Name = "External SSD 1TB",  ImgUri = "https://img/11", Price = 140m,  Description = "USB 3.2 Gen2" },
                new() { Id = 12, Name = "External HDD 2TB",  ImgUri = "https://img/12", Price = 85m,   Description = "Portable storage" },
                new() { Id = 13, Name = "Wi-Fi Router",      ImgUri = "https://img/13", Price = 150m,  Description = "Wi-Fi 6 router" },
                new() { Id = 14, Name = "Ethernet Switch",   ImgUri = "https://img/14", Price = 55m,   Description = "8-port gigabit" },
                new() { Id = 15, Name = "Smart Plug",        ImgUri = "https://img/15", Price = 18m,   Description = "Energy monitoring" },
                new() { Id = 16, Name = "Smart Bulb",        ImgUri = "https://img/16", Price = 15m,   Description = "RGB + warm white" },
                new() { Id = 17, Name = "Desk Lamp",         ImgUri = "https://img/17", Price = 35m,   Description = "Adjustable brightness" },
                new() { Id = 18, Name = "Laptop Stand",      ImgUri = "https://img/18", Price = 30m,   Description = "Aluminum stand" },
                new() { Id = 19, Name = "Office Chair Mat",  ImgUri = "https://img/19", Price = 25m,   Description = null },
                new() { Id = 20, Name = "Cable Organizer",   ImgUri = "https://img/20", Price = 12m,   Description = "Under-desk tray" },

                new() { Id = 21, Name = "Power Strip",       ImgUri = "https://img/21", Price = 22m,   Description = "Surge protection" },
                new() { Id = 22, Name = "UPS 650VA",         ImgUri = "https://img/22", Price = 110m,  Description = "Backup power" },
                new() { Id = 23, Name = "HDMI Cable 2m",     ImgUri = "https://img/23", Price = 10m,   Description = "High speed HDMI" },
                new() { Id = 24, Name = "USB-C Cable 1m",    ImgUri = "https://img/24", Price = 9m,    Description = "100W PD cable" },
                new() { Id = 25, Name = "Mouse Pad XL",      ImgUri = "https://img/25", Price = 14m,   Description = "Extended desk mat" },
                new() { Id = 26, Name = "Portable Charger",  ImgUri = "https://img/26", Price = 40m,   Description = "10,000mAh" },
                new() { Id = 27, Name = "Bluetooth Adapter", ImgUri = "https://img/27", Price = 13m,   Description = "USB Bluetooth 5.0" },
                new() { Id = 28, Name = "USB Flash 128GB",   ImgUri = "https://img/28", Price = 16m,   Description = null },
                new() { Id = 29, Name = "Printer",           ImgUri = "https://img/29", Price = 130m,  Description = "Mono laser printer" },
                new() { Id = 30, Name = "Scanner",           ImgUri = "https://img/30", Price = 95m,   Description = "Document scanner" },
            ];

        public IReadOnlyList<ProductDto> GetAll()
            => _products.Select(ToDto).ToList();

        public ProductDto? GetById(int id)
            => _products.Where(p => p.Id == id).Select(ToDto).FirstOrDefault();

        public bool UpdateDescription(int id, string? description)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product is null) return false;

            product.Description = description;
            return true;
        }

        private static ProductDto ToDto(Product p)
            => new(p.Id, p.Name, p.ImgUri, p.Price, p.Description);

        public PagedResponse<ProductDto> GetPaged(int page, int pageSize)
        {
            page = page < 1 ? 1 : page;
            pageSize = pageSize < 1 ? 10 : pageSize;
            pageSize = pageSize > 100 ? 100 : pageSize;

            var totalCount = _products.Count;

            var items = _products
                .OrderBy(p => p.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(ToDto)
                .ToList();

            return new PagedResponse<ProductDto>(items, page, pageSize, totalCount);
        }
    }
}
