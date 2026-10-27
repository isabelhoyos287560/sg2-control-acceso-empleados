using AccessControl.Application.Utilities.Mediator;
using AccessControl.Application.Utilities.Pagination;
using AccessControl.Application.Utilities.Results;
using AccessControl.Domain.Entities.Employees.ValueObjects;

namespace AccessControl.Application.UseCases.Employees.Queries.GetEmployeesList
{
    public sealed class GetEmployeesListQuery : IRequest<Result<PaginationResponse<EmployeeListItemDTO>>>
    {
        public PaginationRequest Pagination { get; set; } = PaginationRequest.Standard();

        public EmploymentStatus? Status { get; set; }
        public string? AreaCode { get; set; }
    }
}
