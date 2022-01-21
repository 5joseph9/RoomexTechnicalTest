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

                await WriteResponse(httpContext, response, StatusCodes.Status400BadRequest);
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
                    response = "An internal error occurred. If the problem persists, contact your administrator";
                }

                await WriteResponse(httpContext, response, StatusCodes.Status500InternalServerError);
                _logger.LogError(exception, exception.Message);

            }
        }

        private async Task WriteResponse(HttpContext httpContext, object response, int statusCode) {
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = statusCode;
            await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}