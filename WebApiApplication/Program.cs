
using WebApiApplication.Interfaces;
using WebApiApplication.Services;
using Microsoft.EntityFrameworkCore;
using WebApiApplication.Data;

namespace WebApiApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //choose "Sql" / "InMemory" in appsettings.json
            var dataSource = builder.Configuration["DataSource"];
            if (string.Equals(dataSource, "Sql", StringComparison.OrdinalIgnoreCase))
            {
                builder.Services.AddDbContext<AppDbContext>(options =>
                    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

                builder.Services.AddScoped<IProductService, EfProductService>();
            }
            else
            {
                builder.Services.AddSingleton<IProductService, InMemoryProductService>();
            }

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
