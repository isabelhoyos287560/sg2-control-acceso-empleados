using AccessControl.Application.Contracts.Persistence;
using AccessControl.Application.Contracts.Repositories;
using AccessControl.Application.Exceptions;
using AccessControl.Application.UseCases.Common;
using AccessControl.Application.Utilities.Mediator;
using AccessControl.Application.Utilities.Results;
using AccessControl.Domain.Common.ValueObjects;
using AccessControl.Domain.Entities.Employees;
using AccessControl.Domain.Entities.Employees.ValueObjects;
using AccessControl.Domain.Exceptions;

namespace AccessControl.Application.UseCases.Employees.Commands.UpdateEmployee
{
    public sealed class UpdateEmployeeUseCase : BaseHandler, IRequestHandler<UpdateEmployeeCommand, Result<Guid>>
    {
        private readonly IEmployeesRepository _employeesRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateEmployeeUseCase(IEmployeesRepository employeesRepository, IUnitOfWork unitOfWork)
        {
            _employeesRepository = employeesRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(UpdateEmployeeCommand command)
        {
            return await ExecuteAsync(async () =>
            {
                Employee? employee = await _employeesRepository.GetByIdAsync(command.Id);

                if (employee is null)
                {
                    throw new NotFoundException($"No existe un empleado con el ID {command.Id}.");
                }

                Email email = Email.Create(command.Email);

                if (await _employeesRepository.ExistsByEmailAsync(email, excludedEmployeeId: employee.Id))
                {
                    throw new BusinessRuleException($"Ya existe otro empleado con el correo {email}.");
                }

                employee.UpdateNames(command.FirstName, command.LastName);
                employee.UpdateEmail(email);
                employee.UpdatePosition(command.Position);
                employee.UpdateArea(OrganizationalArea.FromCode(command.AreaCode));

                await _employeesRepository.UpdateAsync(employee);
                await _unitOfWork.CommitAsync();

                return employee.Id;
            });
        }
    }
}
