using AccessControl.Application.Contracts.Repositories;
using AccessControl.Application.UseCases.Common;
using AccessControl.Application.Utilities.Mediator;
using AccessControl.Application.Utilities.Pagination;
using AccessControl.Application.Utilities.Results;
using AccessControl.Domain.Entities.AccessPoints;

namespace AccessControl.Application.UseCases.AccessPoints.Queries.GetAccessPointsList
{
    public sealed class GetAccessPointsListUseCase :
        BaseHandler,
        IRequestHandler<GetAccessPointsListQuery, Result<PaginationResponse<AccessPointDTO>>>
    {
        private readonly IAccessPointsRepository _accessPointsRepository;

        public GetAccessPointsListUseCase(IAccessPointsRepository accessPointsRepository)
        {
            _accessPointsRepository = accessPointsRepository;
        }

        public async Task<Result<PaginationResponse<AccessPointDTO>>> Handle(GetAccessPointsListQuery query)
        {
            return await ExecuteAsync(async () =>
            {
                PaginationRequest pagination = query.Pagination;

                PaginationResponse<AccessPoint> response = await _accessPointsRepository.GetPagedListAsync(pagination,
                                                                                                           query.IsEnabled,
                                                                                                           query.Type);

                List<AccessPointDTO> items = response.Items.Select(ap => ap.ToDTO())
                                                           .ToList();

                return PaginationResponse<AccessPointDTO>.Create(items, response.TotalCount, pagination);
            });
        }
    }
}
