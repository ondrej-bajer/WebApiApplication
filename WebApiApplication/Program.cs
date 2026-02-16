
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Events;
using Swashbuckle.AspNetCore.SwaggerGen;
using WebApiApplication.Configuration;
using WebApiApplication.Data;
using WebApiApplication.Interfaces;
using WebApiApplication.Middleware;
using WebApiApplication.Services;

namespace WebApiApplication
{
    public partial class Program
    {

        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File("logs/log-.txt", outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}", rollingInterval: RollingInterval.Day)
                .WriteTo.Console()
                .CreateLogger();

            try
            {
                Log.Information("Starting WebApi");

                var builder = WebApplication.CreateBuilder(args);

                builder.Host.UseSerilog();

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
                builder.Services.AddTransient<ExceptionHandlingMiddleware>();
                builder.Services.AddHealthChecks();

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

                app.UseMiddleware<ExceptionHandlingMiddleware>();
                app.UseSerilogRequestLogging();
                app.MapHealthChecks("/health");

                app.UseHttpsRedirection();

                app.MapControllers();

                app.Run();
            }
            
            catch (Exception ex) 
            {
                Log.Fatal(ex, "Application terminated unexpectedly");
            }
            
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }

}
