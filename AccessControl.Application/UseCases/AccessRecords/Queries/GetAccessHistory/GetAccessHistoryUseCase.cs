using AccessControl.Application.Contracts.Repositories;
using AccessControl.Application.UseCases.Common;
using AccessControl.Application.Utilities.Mediator;
using AccessControl.Application.Utilities.Pagination;
using AccessControl.Application.Utilities.Results;
using AccessControl.Domain.Entities.AccessRecords;

namespace AccessControl.Application.UseCases.AccessRecords.Queries.GetAccessHistory
{
    public sealed class GetAccessHistoryUseCase :
        BaseHandler,
        IRequestHandler<GetAccessHistoryQuery, Result<PaginationResponse<AccessRecordListItemDTO>>>
    {
        private readonly IAccessRecordsRepository _accessRecordsRepository;

        public GetAccessHistoryUseCase(IAccessRecordsRepository accessRecordsRepository)
        {
            _accessRecordsRepository = accessRecordsRepository;
        }

        public async Task<Result<PaginationResponse<AccessRecordListItemDTO>>> Handle(GetAccessHistoryQuery query)
        {
            return await ExecuteAsync(async () =>
            {
                PaginationRequest pagination = query.Pagination;

                PaginationResponse<AccessRecord> response = await _accessRecordsRepository.GetPagedHistoryAsync(pagination,
                                                                                                                query.EmployeeId,
                                                                                                                query.From,
                                                                                                                query.To,
                                                                                                                query.AccessPointId,
                                                                                                                query.Result);

                List<AccessRecordListItemDTO> items = response.Items.Select(r => r.ToListItemDTO())
                                                                    .ToList();

                return PaginationResponse<AccessRecordListItemDTO>.Create(items, response.TotalCount, pagination);
            });
        }
    }
}
