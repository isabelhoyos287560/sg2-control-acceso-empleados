using AccessControl.Domain.Entities.Employees.ValueObjects;
using FluentValidation;

namespace AccessControl.Application.UseCases.Employees.Queries.GetEmployeesList
{
    public sealed class GetEmployeesListQueryValidator : AbstractValidator<GetEmployeesListQuery>
    {
        public GetEmployeesListQueryValidator()
        {
            RuleFor(query => query.Status).IsInEnum().When(query => query.Status.HasValue)
                                          .WithMessage("El estado laboral no es válido.");

            RuleFor(query => query.AreaCode).Must(OrganizationalArea.IsSupported).When(query => !string.IsNullOrWhiteSpace(query.AreaCode))
                                            .WithMessage("El código del área organizacional no es válido.");
        }
    }
}
