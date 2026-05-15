using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Middlewares.Handlers
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;
        private readonly IHostEnvironment _env;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger, IHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            // Log the full error in the server console for the developer
            _logger.LogError(exception, "An unhandled exception occurred: {Message}", exception.Message);

            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Server Error",
                Instance = httpContext.Request.Path,
                // If we are in Development or Staging, we show the real error.
                // In Production, we keep it generic for security.
                Detail = _env.IsDevelopment() || _env.IsStaging()
                         ? $"[DEBUG] {exception.Message} | StackTrace: {exception.StackTrace}"
                         : "An unexpected error occurred in the system. Please contact support."
            };

            httpContext.Response.StatusCode = problemDetails.Status.Value;

            // Send the response as a standardized JSON (ProblemDetails)
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }
    }
}