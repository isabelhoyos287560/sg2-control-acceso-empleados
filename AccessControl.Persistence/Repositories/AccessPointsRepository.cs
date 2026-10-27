using AccessControl.Application.Contracts.Repositories;
using AccessControl.Application.Utilities.Pagination;
using AccessControl.Domain.Entities.AccessPoints;
using AccessControl.Domain.Entities.AccessPoints.ValueObjects;
using AccessControl.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;

namespace AccessControl.Persistence.Repositories
{
    public class AccessPointsRepository : Repository<AccessPoint>, IAccessPointsRepository
    {
        private readonly DataContext _context;

        public AccessPointsRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<PaginationResponse<AccessPoint>> GetPagedListAsync(PaginationRequest request,
                                                                             bool? isEnabled,
                                                                             AccessPointType? type,
                                                                             CancellationToken cancellationToken = default)
        {
            IQueryable<AccessPoint> query = _context.AccessPoints.AsNoTracking()
                                                                 .AsQueryable();

            if (isEnabled.HasValue)
            {
                query = query.Where(ap => ap.IsEnabled == isEnabled);
            }

            if (type.HasValue)
            {
                query = query.Where(ap => ap.Type == type);
            }

            query = query.OrderBy(ap => ap.Name);

            return await query.ToPagedListAsync(request, cancellationToken);
        }

        public async Task<bool> ExistsByNameAsync(string name, Guid? excludedAccessPointId = null, CancellationToken cancellationToken = default)
        {
            return await _context.AccessPoints.AnyAsync(ap => ap.Name == name && ap.Id != excludedAccessPointId, cancellationToken);
        }
    }
}
