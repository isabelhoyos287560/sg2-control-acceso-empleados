using AccessControl.Application.Contracts.Persistence;
using AccessControl.Application.Contracts.Repositories;
using AccessControl.Application.Exceptions;
using AccessControl.Application.UseCases.Common;
using AccessControl.Application.Utilities.Mediator;
using AccessControl.Application.Utilities.Results;
using AccessControl.Domain.Entities.Employees;

namespace AccessControl.Application.UseCases.Employees.Commands.DeactivateEmployee
{
    public sealed class DeactivateEmployeeUseCase : BaseHandler, IRequestHandler<DeactivateEmployeeCommand, Result<Guid>>
    {
        private readonly IEmployeesRepository _employeesRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeactivateEmployeeUseCase(IEmployeesRepository employeesRepository, IUnitOfWork unitOfWork)
        {
            _employeesRepository = employeesRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(DeactivateEmployeeCommand command)
        {
            return await ExecuteAsync(async () =>
            {
                Employee? employee = await _employeesRepository.GetByIdAsync(command.Id);

                if (employee is null)
                {
                    throw new NotFoundException($"No existe un empleado con el ID {command.Id}.");
                }

                employee.Deactivate();

                await _employeesRepository.UpdateAsync(employee);
                await _unitOfWork.CommitAsync();

                return employee.Id;
            });
        }
    }
}
