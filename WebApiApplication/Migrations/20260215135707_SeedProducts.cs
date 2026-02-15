using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebApiApplication.Migrations
{
    /// <inheritdoc />
    public partial class SeedProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Description", "ImgUri", "Name", "Price" },
                values: new object[,]
                {
                    { 3, "Mechanical keyboard", "https://img/3", "Keyboard", 120m },
                    { 4, "IPS, 1440p", "https://img/4", "Monitor 27\"", 280m },
                    { 5, "Ultrawide, 144Hz", "https://img/5", "Monitor 34\" UW", 520m },
                    { 6, "HDMI + USB-A + PD", "https://img/6", "USB-C Hub", 45m },
                    { 7, "1080p webcam", "https://img/7", "Webcam", 80m },
                    { 8, "Closed-back headset", "https://img/8", "Headset", 90m },
                    { 9, "USB condenser mic", "https://img/9", "Microphone", 110m },
                    { 10, "2.0 desktop speakers", "https://img/10", "Speakers", 65m },
                    { 11, "USB 3.2 Gen2", "https://img/11", "External SSD 1TB", 140m },
                    { 12, "Portable storage", "https://img/12", "External HDD 2TB", 85m },
                    { 13, "Wi-Fi 6 router", "https://img/13", "Wi-Fi Router", 150m },
                    { 14, "8-port gigabit", "https://img/14", "Ethernet Switch", 55m },
                    { 15, "Energy monitoring", "https://img/15", "Smart Plug", 18m },
                    { 16, "RGB + warm white", "https://img/16", "Smart Bulb", 15m },
                    { 17, "Adjustable brightness", "https://img/17", "Desk Lamp", 35m },
                    { 18, "Aluminum stand", "https://img/18", "Laptop Stand", 30m },
                    { 19, null, "https://img/19", "Office Chair Mat", 25m },
                    { 20, "Under-desk tray", "https://img/20", "Cable Organizer", 12m },
                    { 21, "Surge protection", "https://img/21", "Power Strip", 22m },
                    { 22, "Backup power", "https://img/22", "UPS 650VA", 110m },
                    { 23, "High speed HDMI", "https://img/23", "HDMI Cable 2m", 10m },
                    { 24, "100W PD cable", "https://img/24", "USB-C Cable 1m", 9m },
                    { 25, "Extended desk mat", "https://img/25", "Mouse Pad XL", 14m },
                    { 26, "10,000mAh", "https://img/26", "Portable Charger", 40m },
                    { 27, "USB Bluetooth 5.0", "https://img/27", "Bluetooth Adapter", 13m },
                    { 28, null, "https://img/28", "USB Flash 128GB", 16m },
                    { 29, "Mono laser printer", "https://img/29", "Printer", 130m },
                    { 30, "Document scanner", "https://img/30", "Scanner", 95m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 30);
        }
    }
}
