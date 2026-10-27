using AccessControl.Application.UseCases.Common;
using AccessControl.Application.Utilities.Mediator;
using AccessControl.Application.Utilities.Results;
using AccessControl.Domain.Entities.Employees.ValueObjects;

namespace AccessControl.Application.UseCases.Areas.Queries.GetAreasList
{
    public sealed class GetAreasListUseCase : BaseHandler, IRequestHandler<GetAreasListQuery, Result<List<AreaDTO>>>
    {
        public async Task<Result<List<AreaDTO>>> Handle(GetAreasListQuery query)
        {
            return await ExecuteAsync(() =>
            {
                List<AreaDTO> areas = OrganizationalArea.All.Select(a => new AreaDTO { Code = a.Code, Name = a.Name })
                                                            .ToList();

                return Task.FromResult(areas);
            });
        }
    }
}
