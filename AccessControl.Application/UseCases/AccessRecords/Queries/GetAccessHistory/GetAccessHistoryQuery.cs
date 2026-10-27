using AccessControl.Application.Utilities.Mediator;
using AccessControl.Application.Utilities.Pagination;
using AccessControl.Application.Utilities.Results;
using AccessControl.Domain.Entities.AccessRecords.ValueObjects;

namespace AccessControl.Application.UseCases.AccessRecords.Queries.GetAccessHistory
{
    public sealed class GetAccessHistoryQuery : IRequest<Result<PaginationResponse<AccessRecordListItemDTO>>>
    {
        public PaginationRequest Pagination { get; set; } = PaginationRequest.Standard();

        public Guid? EmployeeId { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public Guid? AccessPointId { get; set; }
        public AccessResult? Result { get; set; }
    }
}
