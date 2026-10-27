using AccessControl.Application.Utilities.Pagination;
using AccessControl.Domain.Entities.AccessPoints;
using AccessControl.Domain.Entities.AccessPoints.ValueObjects;

namespace AccessControl.Application.Contracts.Repositories
{
    public interface IAccessPointsRepository : IRepository<AccessPoint>
    {
        Task<PaginationResponse<AccessPoint>> GetPagedListAsync(PaginationRequest request,
                                                                bool? isEnabled,
                                                                AccessPointType? type,
                                                                CancellationToken cancellationToken = default);

        Task<bool> ExistsByNameAsync(string name, Guid? excludedAccessPointId = null, CancellationToken cancellationToken = default);
    }
}
