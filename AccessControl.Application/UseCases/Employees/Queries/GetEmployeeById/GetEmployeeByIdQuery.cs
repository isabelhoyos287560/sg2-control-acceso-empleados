using AccessControl.Application.Utilities.Mediator;
using AccessControl.Application.Utilities.Results;

namespace AccessControl.Application.UseCases.Employees.Queries.GetEmployeeById
{
    public sealed class GetEmployeeByIdQuery : IRequest<Result<EmployeeDetailsDTO>>
    {
        public Guid Id { get; set; }
    }
}
