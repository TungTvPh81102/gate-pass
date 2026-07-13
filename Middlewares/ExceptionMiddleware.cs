using System.Net;
using System.Text.Json;

namespace BackEnd.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private const string TRACE_ID_KEY = "TraceId";
        private const string TRACE_ID_HEADER = "X-Trace-Id";

        public ExceptionMiddleware(
            RequestDelegate next,
            ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                var traceId = context.Request.Headers[TRACE_ID_HEADER].FirstOrDefault()
                    ?? Guid.NewGuid().ToString();

                context.Items[TRACE_ID_KEY] = traceId;

                context.Response.OnStarting(() =>
                {
                    context.Response.Headers[TRACE_ID_HEADER] = traceId;
                    return Task.CompletedTask;
                });

                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var statusCode = HttpStatusCode.InternalServerError; // 500
            var message = "An unexpected error occurred";

            _logger.LogError(exception, "Unhandled exception occurred");

            switch (exception)
            {
                case ArgumentException argEx:
                    statusCode = HttpStatusCode.BadRequest; // 400
                    message = argEx.Message;
                    _logger.LogWarning("Bad request: {Message}", argEx.Message);
                    break;

                case KeyNotFoundException notFoundEx:
                    statusCode = HttpStatusCode.NotFound; // 404
                    message = notFoundEx.Message;
                    _logger.LogWarning("Resource not found: {Message}", notFoundEx.Message);
                    break;

                case InvalidOperationException invalidOpEx:
                    statusCode = HttpStatusCode.BadRequest; // 400
                    message = invalidOpEx.Message;
                    _logger.LogWarning("Invalid operation: {Message}", invalidOpEx.Message);
                    break;

                case UnauthorizedAccessException unauthEx:
                    statusCode = HttpStatusCode.Unauthorized; // 401
                    message = "Unauthorized";
                    _logger.LogWarning("Unauthorized access: {Message}", unauthEx.Message);
                    break;

                default:
                    _logger.LogCritical(exception, "Critical unhandled exception");
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var result = new
            {
                StatusCode = (int)statusCode,
                Message = message,
            };

            var json = JsonSerializer.Serialize(result);
            await context.Response.WriteAsync(json);

        }
    }
}
