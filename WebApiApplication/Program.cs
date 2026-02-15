
using WebApiApplication.Interfaces;
using WebApiApplication.Services;
using Microsoft.EntityFrameworkCore;
using WebApiApplication.Data;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using WebApiApplication.Configuration;

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
            builder.Services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
            })
            .AddMvc()
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });


            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigSwaggerOptions>();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint(
                            $"/swagger/{description.GroupName}/swagger.json",
                            description.GroupName.ToUpperInvariant());
                    }
                });
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
