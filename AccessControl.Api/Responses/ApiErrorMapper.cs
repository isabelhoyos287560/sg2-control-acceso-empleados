using AccessControl.Application.Utilities.Results;

namespace AccessControl.Api.Responses
{
    internal static class ApiErrorMapper
    {
        public static ApiError From(Error error)
        {
            return new ApiError(error.Code, error.Description, MapType(error.Type));
        }

        public static IReadOnlyList<ApiError> FromMany(IReadOnlyCollection<Error> errors)
        {
            if (errors.Count == 0)
            {
                return [];
            }

            return errors.Select(From).ToList();
        }

        private static string MapType(ErrorType type)
        {
            return type switch
            {
                ErrorType.None => "none",
                ErrorType.Validation => "validation",
                ErrorType.Business => "business",
                ErrorType.NotFound => "notfound",
                ErrorType.Unexpected => "unexpected",
                _ => "unexpected",
            };
        }
    }
}
