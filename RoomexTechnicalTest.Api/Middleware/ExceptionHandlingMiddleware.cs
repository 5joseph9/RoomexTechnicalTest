using System.Text.Json;
using FluentValidation;
using RoomexTechnicalTest.Application.Models.Validation;

namespace RoomexTechnicalTest.Api.Middleware {
    public class ExceptionHandlingMiddleware {
        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _env;
        private readonly ILogger _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, IHostEnvironment env, ILogger<ExceptionHandlingMiddleware> logger) {
            _next = next;
            _env = env;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context) {
            try {
                await _next(context);
            }
            catch (Exception e) {
                await OnException(context, e);
            }
        }

        public async Task OnException(HttpContext httpContext, Exception exception) {
            if (exception is ValidationException validationException) {
                var response = new {
                    Message = "One or more validation errors occurred.",
                    Details = validationException.Errors.Select(error => new ValidationError(error.PropertyName,
                        error.ErrorMessage,
                        error.AttemptedValue))
                };

                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
            else {
                object response;

                if (_env.IsDevelopment()) {
                    response = new {
                        exception.Message,
                        exception.StackTrace
                    };
                }
                else {
                    response = $"A server error occurred: {exception.Message}";
                    _logger.LogError(exception, exception.Message);
                }

                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }
    }
}