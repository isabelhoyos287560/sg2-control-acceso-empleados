using AccessControl.Application.Contracts.Persistence;
using AccessControl.Application.Contracts.Repositories;
using AccessControl.Application.UseCases.Common;
using AccessControl.Application.Utilities.Mediator;
using AccessControl.Application.Utilities.Results;
using AccessControl.Domain.Common.ValueObjects;
using AccessControl.Domain.Entities.Employees;
using AccessControl.Domain.Entities.Employees.ValueObjects;
using AccessControl.Domain.Exceptions;

namespace AccessControl.Application.UseCases.Employees.Commands.CreateEmployee
{
    public sealed class CreateEmployeeUseCase : BaseHandler, IRequestHandler<CreateEmployeeCommand, Result<Guid>>
    {
        private readonly IEmployeesRepository _employeesRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateEmployeeUseCase(IEmployeesRepository employeesRepository, IUnitOfWork unitOfWork)
        {
            _employeesRepository = employeesRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(CreateEmployeeCommand command)
        {
            return await ExecuteAsync(async () =>
            {
                Email email = Email.Create(command.Email);

                if (await _employeesRepository.ExistsByIdentificationNumberAsync(command.IdentificationNumber.Trim()))
                {
                    throw new BusinessRuleException($"Ya existe un empleado con la identificación {command.IdentificationNumber}.");
                }

                if (await _employeesRepository.ExistsByEmailAsync(email))
                {
                    throw new BusinessRuleException($"Ya existe un empleado con el correo {email}.");
                }

                Employee employee = new(command.IdentificationNumber,
                                        command.FirstName,
                                        command.LastName,
                                        email,
                                        command.Position,
                                        OrganizationalArea.FromCode(command.AreaCode));

                await _employeesRepository.CreateAsync(employee);
                await _unitOfWork.CommitAsync();

                return employee.Id;
            });
        }
    }
}
