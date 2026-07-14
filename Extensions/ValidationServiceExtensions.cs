using System.Reflection;
using BackEnd.Common;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Extensions
{
    public static class ValidationServiceExtensions
    {
        public static IServiceCollection AddCustomValidation(this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation();

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    var traceId = context.HttpContext.Items["TraceId"]?.ToString()
                                  ?? Guid.NewGuid().ToString();

                    return new BadRequestObjectResult(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Validation failed",
                        Errors = errors,
                        TraceId = traceId
                    });
                };
            });

            return services;
        }
    }
}