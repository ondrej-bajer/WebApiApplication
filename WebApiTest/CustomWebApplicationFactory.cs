using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace WebApiTest;

public sealed class CustomWebApplicationFactory : WebApplicationFactory<WebApiApplication.Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((context, config) =>
        {
            var settings = new Dictionary<string, string?>
            {
                ["DataSource"] = "InMemory",

                // JWT config used BOTH for token issuing and validation
                ["Jwt:Issuer"] = "WebApiApplication",
                ["Jwt:Audience"] = "WebApiApplication",
                ["Jwt:Key"] = "TEST_SECRET_KEY_CHANGE_ME_TO_32+_CHARS_LONG_123456",
                ["Jwt:ExpiresMinutes"] = "60",

                // Demo users for login
                ["DemoUsers:Users:0:Username"] = "admin",
                ["DemoUsers:Users:0:Password"] = "admin123",
                ["DemoUsers:Users:0:Role"] = "Admin",

                ["DemoUsers:Users:1:Username"] = "user",
                ["DemoUsers:Users:1:Password"] = "user123",
                ["DemoUsers:Users:1:Role"] = "User",
            };

            config.AddInMemoryCollection(settings);
        });
    }
}