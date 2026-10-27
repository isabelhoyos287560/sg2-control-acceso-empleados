using AccessControl.Api.Responses;
using AccessControl.Application.Exceptions;

namespace AccessControl.Api.Middlewares
{
    public sealed class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (CustomValidationException ex)
            {
                IReadOnlyList<ApiError> errors = ex.Errors.Select(error => new ApiError($"Validation.{error.PropertyName}",
                                                                                         error.ErrorMessage,
                                                                                         "validation"))
                                                          .ToList();

                await WriteAsync(context, StatusCodes.Status400BadRequest, errors, "Debe ajustar los errores de validación");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error no controlado procesando {Path}", context.Request.Path);

                IReadOnlyList<ApiError> errors = [new ApiError("Unexpected", "Unexpected error occurred", "unexpected")];

                await WriteAsync(context, StatusCodes.Status500InternalServerError, errors, "Ha ocurrido un error");
            }
        }

        private static async Task WriteAsync(HttpContext context, int statusCode, IReadOnlyList<ApiError> errors, string message)
        {
            context.Response.StatusCode = statusCode;
            await context.Response.WriteAsJsonAsync(ApiResponse<object>.Fail(errors, message));
        }
    }
}
