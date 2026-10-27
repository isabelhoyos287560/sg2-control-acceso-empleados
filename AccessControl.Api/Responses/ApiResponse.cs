namespace AccessControl.Api.Responses
{
    public sealed class ApiResponse<T>
    {
        public bool IsSuccess { get; init; }
        public T? Data { get; init; }
        public string? Message { get; init; }
        public IReadOnlyCollection<ApiError>? Errors { get; init; }

        public static ApiResponse<T> Ok(T data, string? message = null)
        {
            return new ApiResponse<T>
            {
                IsSuccess = true,
                Data = data,
                Message = message,
                Errors = null,
            };
        }

        public static ApiResponse<T> Fail(IReadOnlyCollection<ApiError> errors, string? message = null)
        {
            return new ApiResponse<T>
            {
                IsSuccess = false,
                Data = default,
                Message = message,
                Errors = errors,
            };
        }
    }
}
