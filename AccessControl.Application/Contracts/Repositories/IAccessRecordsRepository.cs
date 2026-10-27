using AccessControl.Application.Utilities.Pagination;
using AccessControl.Domain.Entities.AccessRecords;
using AccessControl.Domain.Entities.AccessRecords.ValueObjects;

namespace AccessControl.Application.Contracts.Repositories
{
    public interface IAccessRecordsRepository : IRepository<AccessRecord>
    {
        Task<MovementType?> GetLastAuthorizedMovementAsync(Guid employeeId, CancellationToken cancellationToken = default);

        Task<PaginationResponse<AccessRecord>> GetPagedHistoryAsync(PaginationRequest request,
                                                                    Guid? employeeId,
                                                                    DateTime? from,
                                                                    DateTime? to,
                                                                    Guid? accessPointId,
                                                                    AccessResult? result,
                                                                    CancellationToken cancellationToken = default);
    }
}
