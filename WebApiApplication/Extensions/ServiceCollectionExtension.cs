using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using WebApiApplication.Configuration;

namespace WebApiApplication.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddApi(this IServiceCollection services, IConfiguration config)
        {
            services.AddControllers();

            services.AddOptions<JwtOptions>()
                .Bind(config.GetSection(JwtOptions.SectionName))
                .Validate(o =>
                    !string.IsNullOrWhiteSpace(o.Issuer) &&
                    !string.IsNullOrWhiteSpace(o.Audience) &&
                    !string.IsNullOrWhiteSpace(o.Key),
                    "Jwt options are not configured properly.")
                .ValidateOnStart();

            // ✅ načti hodnoty jednou z IConfiguration
            var jwt = config.GetSection(JwtOptions.SectionName).Get<JwtOptions>()
                      ?? throw new InvalidOperationException("Jwt section missing.");

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,

                        ValidIssuer = jwt.Issuer,
                        ValidAudience = jwt.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key)),
                        ClockSkew = TimeSpan.FromSeconds(30)
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("CanWriteProducts", p => p.RequireRole("Admin"));
            });

            services.AddSwaggerGen(c =>
            {
                var scheme = new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter: Bearer {your JWT token}"
                };

                c.AddSecurityDefinition("Bearer", scheme);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { scheme, Array.Empty<string>() }
                });
            });

            return services;
        }
    }
}