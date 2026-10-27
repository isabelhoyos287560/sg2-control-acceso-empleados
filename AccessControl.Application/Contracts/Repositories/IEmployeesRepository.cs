using AccessControl.Application.Utilities.Pagination;
using AccessControl.Domain.Common.ValueObjects;
using AccessControl.Domain.Entities.Employees;
using AccessControl.Domain.Entities.Employees.ValueObjects;

namespace AccessControl.Application.Contracts.Repositories
{
    public interface IEmployeesRepository : IRepository<Employee>
    {
        Task<PaginationResponse<Employee>> GetPagedListAsync(PaginationRequest request,
                                                             EmploymentStatus? status,
                                                             OrganizationalArea? area,
                                                             CancellationToken cancellationToken = default);

        Task<Employee?> GetByIdWithCredentialsAsync(Guid id, CancellationToken cancellationToken = default);

        Task<Employee?> GetByCredentialCodeAsync(CredentialCode code, CancellationToken cancellationToken = default);

        Task<bool> ExistsByIdentificationNumberAsync(string identificationNumber, CancellationToken cancellationToken = default);

        Task<bool> ExistsByEmailAsync(Email email, Guid? excludedEmployeeId = null, CancellationToken cancellationToken = default);

        Task<bool> ExistsCredentialCodeAsync(CredentialCode code, CancellationToken cancellationToken = default);
    }
}
