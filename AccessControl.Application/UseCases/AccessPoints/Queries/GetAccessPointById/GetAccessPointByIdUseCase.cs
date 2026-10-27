using AccessControl.Application.Contracts.Repositories;
using AccessControl.Application.Exceptions;
using AccessControl.Application.UseCases.AccessPoints.Queries.GetAccessPointsList;
using AccessControl.Application.UseCases.Common;
using AccessControl.Application.Utilities.Mediator;
using AccessControl.Application.Utilities.Results;
using AccessControl.Domain.Entities.AccessPoints;

namespace AccessControl.Application.UseCases.AccessPoints.Queries.GetAccessPointById
{
    public sealed class GetAccessPointByIdUseCase : BaseHandler, IRequestHandler<GetAccessPointByIdQuery, Result<AccessPointDTO>>
    {
        private readonly IAccessPointsRepository _accessPointsRepository;

        public GetAccessPointByIdUseCase(IAccessPointsRepository accessPointsRepository)
        {
            _accessPointsRepository = accessPointsRepository;
        }

        public async Task<Result<AccessPointDTO>> Handle(GetAccessPointByIdQuery query)
        {
            return await ExecuteAsync(async () =>
            {
                AccessPoint? accessPoint = await _accessPointsRepository.GetByIdAsync(query.Id);

                if (accessPoint is null)
                {
                    throw new NotFoundException($"No existe un punto de acceso con el ID {query.Id}.");
                }

                return accessPoint.ToDTO();
            });
        }
    }
}
