using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace WebApiApplication.Middleware
{
    public sealed class ApiExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<ApiExceptionHandler> _logger;
        private readonly IHostEnvironment _env;

        public ApiExceptionHandler(ILogger<ApiExceptionHandler> logger, IHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }

        public async ValueTask<bool> TryHandleAsync(
    HttpContext httpContext,
    Exception exception,
    CancellationToken cancellationToken)
        {
            if (httpContext.Response.HasStarted)
            {
                _logger.LogWarning(exception,
                    "Response already started, cannot write ProblemDetails. TraceId={TraceId}",
                    GetTraceId(httpContext));

                return false; //default handling via ASP.NET
            }

            var (status, title, type) = Map(exception);

            // logs: 5xx = Error, 4xx = Warning
            if (status >= 500)
                _logger.LogError(exception, "Unhandled exception. TraceId={TraceId}", GetTraceId(httpContext));
            else
                _logger.LogWarning(exception, "Request failed with expected error. TraceId={TraceId}", GetTraceId(httpContext));

            var problem = new ProblemDetails
            {
                Status = status,
                Title = title,
                Type = type,
                Instance = httpContext.Request.Path
            };

            if (_env.IsDevelopment())
                problem.Detail = exception.Message;

            // Extensions
            problem.Extensions["traceId"] = GetTraceId(httpContext);
            problem.Extensions["timestamp"] = DateTimeOffset.Now; // (3)

            httpContext.Response.Clear();
            httpContext.Response.StatusCode = status;

            // right content type
            httpContext.Response.ContentType = "application/problem+json";

            await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);

            return true;
        }

        private static (int status, string title, string type) Map(Exception ex) =>
            ex switch
            {
                // 400
                ValidationException or ArgumentException or FormatException =>
                    (StatusCodes.Status400BadRequest, "Bad Request", "https://httpstatuses.com/400"),

                // 404
                KeyNotFoundException =>
                    (StatusCodes.Status404NotFound, "Not Found", "https://httpstatuses.com/404"),

                // 409
                DbUpdateConcurrencyException =>
                    (StatusCodes.Status409Conflict, "Conflict", "https://httpstatuses.com/409"),

                DbUpdateException =>
                    (StatusCodes.Status409Conflict, "Conflict", "https://httpstatuses.com/409"),

                // 401/403
                UnauthorizedAccessException =>
                    (StatusCodes.Status401Unauthorized, "Unauthorized", "https://httpstatuses.com/401"),

                // default 500
                _ =>
                    (StatusCodes.Status500InternalServerError, "An unexpected error occurred.", "https://httpstatuses.com/500")
            };

        private static string GetTraceId(HttpContext ctx) =>
            Activity.Current?.Id ?? ctx.TraceIdentifier;

    }
}
