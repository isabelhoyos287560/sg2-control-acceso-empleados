using AccessControl.Application.UseCases.Credentials.Common;
using FluentValidation;

namespace AccessControl.Application.UseCases.Credentials.Commands.AssignCredential
{
    public sealed class AssignCredentialCommandValidator : AbstractValidator<AssignCredentialCommand>
    {
        public AssignCredentialCommandValidator()
        {
            RuleFor(cmd => cmd.EmployeeId).NotEmpty().WithMessage("El empleado es requerido.");

            RuleFor(cmd => cmd.Code).ValidCredentialCode();
        }
    }
}
