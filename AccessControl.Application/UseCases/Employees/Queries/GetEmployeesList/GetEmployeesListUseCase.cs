using AccessControl.Application.Contracts.Repositories;
using AccessControl.Application.UseCases.Common;
using AccessControl.Application.Utilities.Mediator;
using AccessControl.Application.Utilities.Pagination;
using AccessControl.Application.Utilities.Results;
using AccessControl.Domain.Entities.Employees;
using AccessControl.Domain.Entities.Employees.ValueObjects;

namespace AccessControl.Application.UseCases.Employees.Queries.GetEmployeesList
{
    public sealed class GetEmployeesListUseCase :
        BaseHandler,
        IRequestHandler<GetEmployeesListQuery, Result<PaginationResponse<EmployeeListItemDTO>>>
    {
        private readonly IEmployeesRepository _employeesRepository;

        public GetEmployeesListUseCase(IEmployeesRepository employeesRepository)
        {
            _employeesRepository = employeesRepository;
        }

        public async Task<Result<PaginationResponse<EmployeeListItemDTO>>> Handle(GetEmployeesListQuery query)
        {
            return await ExecuteAsync(async () =>
            {
                PaginationRequest pagination = query.Pagination;

                OrganizationalArea? area = string.IsNullOrWhiteSpace(query.AreaCode)
                    ? null
                    : OrganizationalArea.FromCode(query.AreaCode);

                PaginationResponse<Employee> response = await _employeesRepository.GetPagedListAsync(pagination,
                                                                                                     query.Status,
                                                                                                     area);

                List<EmployeeListItemDTO> items = response.Items.Select(e => e.ToListItemDTO())
                                                                .ToList();

                return PaginationResponse<EmployeeListItemDTO>.Create(items, response.TotalCount, pagination);
            });
        }
    }
}
