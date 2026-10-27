using AccessControl.Application.Contracts.Persistence;
using AccessControl.Application.Contracts.Repositories;
using AccessControl.Application.UseCases.Common;
using AccessControl.Application.Utilities.Mediator;
using AccessControl.Application.Utilities.Results;
using AccessControl.Domain.Entities.AccessPoints;
using AccessControl.Domain.Exceptions;

namespace AccessControl.Application.UseCases.AccessPoints.Commands.CreateAccessPoint
{
    public sealed class CreateAccessPointUseCase : BaseHandler, IRequestHandler<CreateAccessPointCommand, Result<Guid>>
    {
        private readonly IAccessPointsRepository _accessPointsRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateAccessPointUseCase(IAccessPointsRepository accessPointsRepository, IUnitOfWork unitOfWork)
        {
            _accessPointsRepository = accessPointsRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(CreateAccessPointCommand command)
        {
            return await ExecuteAsync(async () =>
            {
                if (await _accessPointsRepository.ExistsByNameAsync(command.Name.Trim()))
                {
                    throw new BusinessRuleException($"Ya existe un punto de acceso con el nombre {command.Name}.");
                }

                AccessPoint accessPoint = new(command.Name, command.Description, command.Type, command.IsEnabled);

                await _accessPointsRepository.CreateAsync(accessPoint);
                await _unitOfWork.CommitAsync();

                return accessPoint.Id;
            });
        }
    }
}
