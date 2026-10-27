using AccessControl.Application.Contracts.Repositories;
using AccessControl.Application.Utilities.Pagination;
using AccessControl.Domain.Entities.AccessRecords;
using AccessControl.Domain.Entities.AccessRecords.ValueObjects;
using AccessControl.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;

namespace AccessControl.Persistence.Repositories
{
    public class AccessRecordsRepository : Repository<AccessRecord>, IAccessRecordsRepository
    {
        private readonly DataContext _context;

        public AccessRecordsRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<MovementType?> GetLastAuthorizedMovementAsync(Guid employeeId, CancellationToken cancellationToken = default)
        {
            return await _context.AccessRecords.AsNoTracking()
                                               .Where(r => r.EmployeeId == employeeId && r.Result == AccessResult.Authorized)
                                               .OrderByDescending(r => r.OccurredAt)
                                               .Select(r => (MovementType?)r.MovementType)
                                               .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<PaginationResponse<AccessRecord>> GetPagedHistoryAsync(PaginationRequest request,
                                                                                 Guid? employeeId,
                                                                                 DateTime? from,
                                                                                 DateTime? to,
                                                                                 Guid? accessPointId,
                                                                                 AccessResult? result,
                                                                                 CancellationToken cancellationToken = default)
        {
            IQueryable<AccessRecord> query = _context.AccessRecords.AsNoTracking()
                                                                   .Include(r => r.Employee)
                                                                   .Include(r => r.AccessPoint)
                                                                   .AsQueryable();

            if (employeeId.HasValue)
            {
                query = query.Where(r => r.EmployeeId == employeeId);
            }

            if (from.HasValue)
            {
                query = query.Where(r => r.OccurredAt >= from);
            }

            if (to.HasValue)
            {
                query = query.Where(r => r.OccurredAt <= to);
            }

            if (accessPointId.HasValue)
            {
                query = query.Where(r => r.AccessPointId == accessPointId);
            }

            if (result.HasValue)
            {
                query = query.Where(r => r.Result == result);
            }

            query = query.OrderByDescending(r => r.OccurredAt);

            return await query.ToPagedListAsync(request, cancellationToken);
        }
    }
}
