using AccessControl.Application.Contracts.Persistence;
using AccessControl.Application.Contracts.Repositories;
using AccessControl.Application.Exceptions;
using AccessControl.Application.UseCases.Common;
using AccessControl.Application.Utilities.Mediator;
using AccessControl.Application.Utilities.Results;
using AccessControl.Domain.Entities.Employees;
using AccessControl.Domain.Entities.Employees.ValueObjects;
using AccessControl.Domain.Exceptions;

namespace AccessControl.Application.UseCases.Credentials.Commands.AssignCredential
{
    public sealed class AssignCredentialUseCase : BaseHandler, IRequestHandler<AssignCredentialCommand, Result<Guid>>
    {
        private readonly IEmployeesRepository _employeesRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AssignCredentialUseCase(IEmployeesRepository employeesRepository, IUnitOfWork unitOfWork)
        {
            _employeesRepository = employeesRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(AssignCredentialCommand command)
        {
            return await ExecuteAsync(async () =>
            {
                Employee? employee = await _employeesRepository.GetByIdWithCredentialsAsync(command.EmployeeId);

                if (employee is null)
                {
                    throw new NotFoundException($"No existe un empleado con el ID {command.EmployeeId}.");
                }

                CredentialCode code = CredentialCode.Create(command.Code);

                // El código es único en todo el sistema, no solo dentro del empleado
                if (await _employeesRepository.ExistsCredentialCodeAsync(code))
                {
                    throw new BusinessRuleException($"Ya existe una credencial con el código {code}.");
                }

                Credential credential = employee.AssignCredential(code);

                // El empleado está siendo rastreado por el DataContext: el UnitOfWork persiste la nueva credencial
                await _unitOfWork.CommitAsync();

                return credential.Id;
            });
        }
    }
}
