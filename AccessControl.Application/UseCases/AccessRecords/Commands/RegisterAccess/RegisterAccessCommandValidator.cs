using AccessControl.Application.UseCases.Credentials.Common;
using FluentValidation;

namespace AccessControl.Application.UseCases.AccessRecords.Commands.RegisterAccess
{
    public sealed class RegisterAccessCommandValidator : AbstractValidator<RegisterAccessCommand>
    {
        public RegisterAccessCommandValidator()
        {
            RuleFor(cmd => cmd.CredentialCode).ValidCredentialCode();

            RuleFor(cmd => cmd.AccessPointId).NotEmpty().WithMessage("El punto de acceso es requerido.");

            RuleFor(cmd => cmd.MovementType).IsInEnum().WithMessage("El tipo de movimiento no es válido (1 = Entrada, 2 = Salida).");
        }
    }
}
