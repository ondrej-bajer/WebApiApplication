using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using WebApiApplication.Configuration;
using WebApiApplication.Data;
using WebApiApplication.Interfaces;
using WebApiApplication.Middleware;
using WebApiApplication.Services;


namespace WebApiApplication.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        // choose "Sql" / "InMemory" in appsettings.json
        var dataSource = config["DataSource"];

        if (string.Equals(dataSource, "Sql", StringComparison.OrdinalIgnoreCase))
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

            services.AddScoped<IProductService, EfProductService>();
        }
        else
        {
            services.AddSingleton<IProductService, InMemoryProductService>();
        }

        services.AddHealthChecks();

        return services;
    }

    public static IServiceCollection AddApi(this IServiceCollection services)
    {
        services.AddControllers();

        // Unified 400 validation response (ValidationProblemDetails + traceId + timestamp)
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var problem = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Validation failed",
                    Type = "https://httpstatuses.com/400",
                    Detail = "One or more validation errors occurred.",
                    Instance = context.HttpContext.Request.Path
                };

                problem.Extensions["traceId"] =
                    System.Diagnostics.Activity.Current?.Id ?? context.HttpContext.TraceIdentifier;

                problem.Extensions["timestamp"] = DateTimeOffset.UtcNow;

                return new BadRequestObjectResult(problem)
                {
                    ContentTypes = { "application/problem+json" }
                };
            };
        });

        services.AddApiVersioning(options =>
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

        services.AddEndpointsApiExplorer();
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigSwaggerOptions>();
        services.AddSwaggerGen();

        services.AddExceptionHandler<ApiExceptionHandler>();
        services.AddProblemDetails();

        return services;
    }
}