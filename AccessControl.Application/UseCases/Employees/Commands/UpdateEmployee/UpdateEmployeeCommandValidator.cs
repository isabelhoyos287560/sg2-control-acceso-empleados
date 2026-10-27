using AccessControl.Domain.Entities.Employees.ValueObjects;
using FluentValidation;

namespace AccessControl.Application.UseCases.Employees.Commands.UpdateEmployee
{
    public sealed class UpdateEmployeeCommandValidator : AbstractValidator<UpdateEmployeeCommand>
    {
        public UpdateEmployeeCommandValidator()
        {
            RuleFor(cmd => cmd.Id).NotEmpty().WithMessage("El ID del empleado es requerido.");

            RuleFor(cmd => cmd.FirstName).NotEmpty().WithMessage("Los nombres son requeridos.")
                                         .Length(2, 64).WithMessage("Los nombres deben tener entre 2 y 64 caracteres.");

            RuleFor(cmd => cmd.LastName).NotEmpty().WithMessage("Los apellidos son requeridos.")
                                        .Length(2, 64).WithMessage("Los apellidos deben tener entre 2 y 64 caracteres.");

            RuleFor(cmd => cmd.Email).NotEmpty().WithMessage("El correo electrónico es requerido.")
                                     .EmailAddress().WithMessage("El correo electrónico no tiene un formato válido.")
                                     .MaximumLength(128).WithMessage("El correo electrónico no puede exceder 128 caracteres.");

            RuleFor(cmd => cmd.Position).NotEmpty().WithMessage("El cargo es requerido.")
                                        .Length(2, 64).WithMessage("El cargo debe tener entre 2 y 64 caracteres.");

            RuleFor(cmd => cmd.AreaCode).Must(OrganizationalArea.IsSupported).WithMessage("El código del área organizacional no es válido.");
        }
    }
}
