using FluentValidation.Results;

namespace AccessControl.Application.Exceptions
{
    public class CustomValidationException : Exception
    {
        public IReadOnlyCollection<ValidationFailure> Errors { get; set; } = [];

        public CustomValidationException(IReadOnlyCollection<ValidationFailure> failures)
        {
            Errors = failures;
        }
    }
}