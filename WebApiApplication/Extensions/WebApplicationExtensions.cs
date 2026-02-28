using Asp.Versioning.ApiExplorer;
using Serilog;

namespace WebApiApplication.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication UseApiPipeline(this WebApplication app)
    {
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

        app.UseExceptionHandler();

        app.UseSerilogRequestLogging(options =>
        {
            options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} => {StatusCode} in {Elapsed:0.0000} ms";

            options.GetLevel = (ctx, elapsed, ex) =>
            {
                var path = ctx.Request.Path.Value ?? "";

                // ignore swagger / framework noise
                if (path.StartsWith("/swagger") ||
                    path.StartsWith("/_framework") ||
                    path.StartsWith("/_vs"))
                {
                    return Serilog.Events.LogEventLevel.Debug;
                }

                if (ex != null || ctx.Response.StatusCode >= 500)
                    return Serilog.Events.LogEventLevel.Error;

                if (ctx.Response.StatusCode >= 400)
                    return Serilog.Events.LogEventLevel.Warning;

                return Serilog.Events.LogEventLevel.Information;
            };
        });

        app.MapHealthChecks("/health");

        app.UseHttpsRedirection();
        app.MapControllers();

        return app;
    }
}