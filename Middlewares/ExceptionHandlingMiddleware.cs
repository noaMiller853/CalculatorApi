using System.Net;
using System.Text.Json;

namespace CalculatorApi.Middleware
{
    /// <summary>
    /// Middleware Pattern - handles HTTP requests during pipeline processing.
    /// Decorator Pattern - extends request pipeline behavior with exception handling.
    /// </summary>
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// Intercepts and wraps request execution in exception handling logic.
        /// </summary>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); // Proceed to the next middleware
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception");
                await HandleExceptionAsync(context, ex);
            }
        }

        /// <summary>
        /// Generates a proper JSON error response based on the exception type.
        /// </summary>
        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = exception switch
            {
                ArgumentException => (int)HttpStatusCode.BadRequest,
                _ => (int)HttpStatusCode.InternalServerError
            };

            var errorResponse = new
            {
                error = exception.Message,
                statusCode = context.Response.StatusCode
            };

            var json = JsonSerializer.Serialize(errorResponse);
            return context.Response.WriteAsync(json);
        }
    }
}
