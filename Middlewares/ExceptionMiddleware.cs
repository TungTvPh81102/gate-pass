using System.Net;
using System.Text.Json;
using BackEnd.Common;

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
            var traceId = context.Items[TRACE_ID_KEY]?.ToString() ?? Guid.NewGuid().ToString();
            var statusCode = HttpStatusCode.InternalServerError;
            var message = "An unexpected error occurred";
            List<string>? errors = null;

            _logger.LogError(exception, "Unhandled exception occurred");

            switch (exception)
            {
                case FluentValidation.ValidationException validationEx:
                    statusCode = HttpStatusCode.BadRequest;
                    message = "Validation failed";
                    errors = validationEx.Errors.Select(e => e.ErrorMessage).ToList();
                    _logger.LogWarning("Validation failed: {Errors}", string.Join(", ", errors));
                    break;

                case ArgumentException argEx:
                    statusCode = HttpStatusCode.BadRequest;
                    message = argEx.Message;
                    _logger.LogWarning("Bad request: {Message}", argEx.Message);
                    break;

                case KeyNotFoundException notFoundEx:
                    statusCode = HttpStatusCode.NotFound;
                    message = notFoundEx.Message;
                    _logger.LogWarning("Resource not found: {Message}", notFoundEx.Message);
                    break;

                case InvalidOperationException invalidOpEx:
                    statusCode = HttpStatusCode.Conflict;
                    message = invalidOpEx.Message;
                    _logger.LogWarning("Invalid operation: {Message}", invalidOpEx.Message);
                    break;

                case UnauthorizedAccessException:
                    statusCode = HttpStatusCode.Unauthorized;
                    message = "Unauthorized";
                    _logger.LogWarning("Unauthorized access");
                    break;

                default:
                    _logger.LogCritical(exception, "Critical unhandled exception");
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var result = new ApiResponse<object>
            {
                Success = false,
                Message = message,
                Errors = errors,
                TraceId = traceId,
            };

            var json = JsonSerializer.Serialize(result, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(json);
        }
    }
}