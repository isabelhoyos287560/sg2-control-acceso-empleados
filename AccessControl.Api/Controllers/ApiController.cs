using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AccessControl.Api.Responses;
using AccessControl.Application.Utilities.Results;

namespace AccessControl.Api.Controllers
{
    [ApiController]
    public abstract class ApiController : ControllerBase
    {
        protected IActionResult ToActionResult<T>(Result<T> result, int successStatusCode = StatusCodes.Status200OK)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblemFromModelState<T>();
            }

            if (result.IsFailure)
            {
                return ToErrorActionResult<T>(result.Errors, result.Message);
            }

            return StatusCode(successStatusCode, ApiResponse<T>.Ok(result.Value, result.Message));
        }

        private IActionResult ToErrorActionResult<T>(IReadOnlyCollection<Error> errors, string? message)
        {
            ErrorType dominantType = errors.FirstOrDefault()?.Type ?? ErrorType.Unexpected;
            ApiResponse<T> body = ApiResponse<T>.Fail(ApiErrorMapper.FromMany(errors), message);

            return dominantType switch
            {
                ErrorType.Validation => StatusCode(StatusCodes.Status400BadRequest, body),
                ErrorType.Business => StatusCode(StatusCodes.Status400BadRequest, body),
                ErrorType.NotFound => StatusCode(StatusCodes.Status404NotFound, body),
                _ => StatusCode(StatusCodes.Status500InternalServerError, body),
            };
        }

        private IActionResult ValidationProblemFromModelState<T>()
        {
            IReadOnlyList<ApiError> apiErrors = ModelState.Values.SelectMany(v => v.Errors)
                                                                 .Select(e => new ApiError("ValidationError", e.ErrorMessage, "validation"))
                                                                 .ToList();

            return StatusCode(StatusCodes.Status400BadRequest, ApiResponse<T>.Fail(apiErrors, "Debe ajustar los errores de validación"));
        }
    }
}
