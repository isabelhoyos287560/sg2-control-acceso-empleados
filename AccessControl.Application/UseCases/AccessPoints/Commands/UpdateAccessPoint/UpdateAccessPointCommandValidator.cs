using FluentValidation;

namespace AccessControl.Application.UseCases.AccessPoints.Commands.UpdateAccessPoint
{
    public sealed class UpdateAccessPointCommandValidator : AbstractValidator<UpdateAccessPointCommand>
    {
        public UpdateAccessPointCommandValidator()
        {
            RuleFor(cmd => cmd.Id).NotEmpty().WithMessage("El ID del punto de acceso es requerido.");

            RuleFor(cmd => cmd.Name).NotEmpty().WithMessage("El nombre del punto de acceso es requerido.")
                                    .Length(3, 64).WithMessage("El nombre del punto de acceso debe tener entre 3 y 64 caracteres.");

            RuleFor(cmd => cmd.Description).MaximumLength(256).WithMessage("La descripción debe tener máximo 256 caracteres.");

            RuleFor(cmd => cmd.Type).IsInEnum().WithMessage("El tipo de punto de acceso no es válido.");
        }
    }
}
