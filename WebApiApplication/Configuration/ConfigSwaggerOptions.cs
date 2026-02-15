using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WebApiApplication.Configuration
{
    public sealed class ConfigSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;

        public ConfigSwaggerOptions(IApiVersionDescriptionProvider provider)
            => _provider = provider;

        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in _provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(
                    description.GroupName,
                    new OpenApiInfo
                    {
                        Title = "WebApiApplication Products API",
                        Version = description.ApiVersion.ToString()
                    });
            }
        }
    }
}
