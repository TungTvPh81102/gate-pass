using BackEnd.Common;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected IActionResult Success<T>(T data, string message = "Success")
        {
            return Ok(new ApiResponse<T>
            {
                Success = true,
                Message = message,
                Data = data,
                TraceId = GetTraceId()
            });
        }

        protected IActionResult Created<T>(T data, string message = "Created successfully")
        {
            return StatusCode(StatusCodes.Status201Created, new ApiResponse<T>
            {
                Success = true,
                Message = message,
                Data = data,
                TraceId = GetTraceId()
            });
        }

        protected IActionResult BadRequest(string message, IEnumerable<string>? errors = null)
        {
            return base.BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = message,
                Errors = errors,
                TraceId = GetTraceId()
            });
        }

        protected IActionResult BadRequestWithModelErrors()
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage);

            return BadRequest("Validation failed", errors);
        }

        protected IActionResult NotFound(string message = "Resource not found")
        {
            return base.NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = message,
                TraceId = GetTraceId()
            });
        }

        protected IActionResult Conflict(string message = "Conflict occurred")
        {
            return base.Conflict(new ApiResponse<object>
            {
                Success = false,
                Message = message,
                TraceId = GetTraceId()
            });
        }

        protected IActionResult UnprocessableEntity(string message, IEnumerable<string>? errors = null)
        {
            return base.UnprocessableEntity(new ApiResponse<object>
            {
                Success = false,
                Message = message,
                Errors = errors,
                TraceId = GetTraceId()
            });
        }

        protected IActionResult InternalServerError(string message = "Internal server error")
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<object>
            {
                Success = false,
                Message = message,
                TraceId = GetTraceId()
            });
        }

        private string GetTraceId()
        {
            return HttpContext.Items["TraceId"]?.ToString()
                   ?? Guid.NewGuid().ToString();
        }
    }
}
