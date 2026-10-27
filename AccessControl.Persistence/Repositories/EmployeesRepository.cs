using AccessControl.Application.Contracts.Repositories;
using AccessControl.Application.Utilities.Pagination;
using AccessControl.Domain.Common.ValueObjects;
using AccessControl.Domain.Entities.Employees;
using AccessControl.Domain.Entities.Employees.ValueObjects;
using AccessControl.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;

namespace AccessControl.Persistence.Repositories
{
    public class EmployeesRepository : Repository<Employee>, IEmployeesRepository
    {
        private readonly DataContext _context;

        public EmployeesRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<PaginationResponse<Employee>> GetPagedListAsync(PaginationRequest request,
                                                                          EmploymentStatus? status,
                                                                          OrganizationalArea? area,
                                                                          CancellationToken cancellationToken = default)
        {
            IQueryable<Employee> query = _context.Employees.AsNoTracking()
                                                           .AsQueryable();

            if (status.HasValue)
            {
                query = query.Where(e => e.Status == status);
            }

            if (area is not null)
            {
                query = query.Where(e => e.Area == area);
            }

            query = query.OrderBy(e => e.LastName)
                         .ThenBy(e => e.FirstName);

            return await query.ToPagedListAsync(request, cancellationToken);
        }

        public async Task<Employee?> GetByIdWithCredentialsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Employees.Include(e => e.Credentials)
                                           .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        }

        public async Task<Employee?> GetByCredentialCodeAsync(CredentialCode code, CancellationToken cancellationToken = default)
        {
            return await _context.Employees.Include(e => e.Credentials)
                                           .FirstOrDefaultAsync(e => e.Credentials.Any(c => c.Code == code), cancellationToken);
        }

        public async Task<bool> ExistsByIdentificationNumberAsync(string identificationNumber, CancellationToken cancellationToken = default)
        {
            return await _context.Employees.AnyAsync(e => e.IdentificationNumber == identificationNumber, cancellationToken);
        }

        public async Task<bool> ExistsByEmailAsync(Email email, Guid? excludedEmployeeId = null, CancellationToken cancellationToken = default)
        {
            return await _context.Employees.AnyAsync(e => e.Email == email && e.Id != excludedEmployeeId, cancellationToken);
        }

        public async Task<bool> ExistsCredentialCodeAsync(CredentialCode code, CancellationToken cancellationToken = default)
        {
            return await _context.Credentials.AnyAsync(c => c.Code == code, cancellationToken);
        }
    }
}
