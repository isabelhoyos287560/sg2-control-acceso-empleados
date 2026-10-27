using AccessControl.Application.Utilities.Mediator;
using AccessControl.Application.Utilities.Results;

namespace AccessControl.Application.UseCases.Areas.Queries.GetAreasList
{
    public sealed class GetAreasListQuery : IRequest<Result<List<AreaDTO>>>
    {
    }
}
