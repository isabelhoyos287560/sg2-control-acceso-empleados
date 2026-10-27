using AccessControl.Application.Utilities.Mediator;
using AccessControl.Application.Utilities.Pagination;
using AccessControl.Application.Utilities.Results;
using AccessControl.Domain.Entities.AccessPoints.ValueObjects;

namespace AccessControl.Application.UseCases.AccessPoints.Queries.GetAccessPointsList
{
    public sealed class GetAccessPointsListQuery : IRequest<Result<PaginationResponse<AccessPointDTO>>>
    {
        public PaginationRequest Pagination { get; set; } = PaginationRequest.Standard();

        public bool? IsEnabled { get; set; }
        public AccessPointType? Type { get; set; }
    }
}
