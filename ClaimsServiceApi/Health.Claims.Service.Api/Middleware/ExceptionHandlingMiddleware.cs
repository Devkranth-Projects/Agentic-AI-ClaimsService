using System.Net;
using System.Text.Json;

namespace Health.Claims.Service.Api.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Pass to next middleware / controller
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var correlationId = context.TraceIdentifier;

            // Log the exception with full details
            _logger.LogError(exception, "Unhandled exception occurred. CorrelationId={CorrelationId}", correlationId);

            // Determine HTTP status code (you can extend for custom exception types)
            var statusCode = exception switch
            {
                ArgumentException => HttpStatusCode.BadRequest,
                InvalidOperationException => HttpStatusCode.NotFound,
                _ => HttpStatusCode.InternalServerError
            };

            // Prepare structured response (RFC 7807)
            var problemDetails = new
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                Title = statusCode == HttpStatusCode.InternalServerError
                            ? "An unexpected error occurred."
                            : exception.Message,
                Status = (int)statusCode,
                Detail = exception.Message,
                Instance = context.Request.Path,
                CorrelationId = correlationId
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var json = JsonSerializer.Serialize(problemDetails);
            await context.Response.WriteAsync(json);
        }
    }
}