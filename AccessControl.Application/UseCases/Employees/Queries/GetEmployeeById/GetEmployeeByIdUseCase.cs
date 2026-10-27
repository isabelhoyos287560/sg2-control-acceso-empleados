using AccessControl.Application.Contracts.Repositories;
using AccessControl.Application.Exceptions;
using AccessControl.Application.UseCases.Common;
using AccessControl.Application.Utilities.Mediator;
using AccessControl.Application.Utilities.Results;
using AccessControl.Domain.Entities.Employees;

namespace AccessControl.Application.UseCases.Employees.Queries.GetEmployeeById
{
    public sealed class GetEmployeeByIdUseCase : BaseHandler, IRequestHandler<GetEmployeeByIdQuery, Result<EmployeeDetailsDTO>>
    {
        private readonly IEmployeesRepository _employeesRepository;

        public GetEmployeeByIdUseCase(IEmployeesRepository employeesRepository)
        {
            _employeesRepository = employeesRepository;
        }

        public async Task<Result<EmployeeDetailsDTO>> Handle(GetEmployeeByIdQuery query)
        {
            return await ExecuteAsync(async () =>
            {
                Employee? employee = await _employeesRepository.GetByIdWithCredentialsAsync(query.Id);

                if (employee is null)
                {
                    throw new NotFoundException($"No existe un empleado con el ID {query.Id}.");
                }

                return employee.ToDetailsDTO();
            });
        }
    }
}
