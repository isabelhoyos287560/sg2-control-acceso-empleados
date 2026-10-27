using AccessControl.Application.Contracts.Persistence;
using AccessControl.Application.Contracts.Repositories;
using AccessControl.Application.Exceptions;
using AccessControl.Application.UseCases.Common;
using AccessControl.Application.Utilities.Mediator;
using AccessControl.Application.Utilities.Results;
using AccessControl.Domain.Entities.AccessPoints;
using AccessControl.Domain.Exceptions;

namespace AccessControl.Application.UseCases.AccessPoints.Commands.UpdateAccessPoint
{
    public sealed class UpdateAccessPointUseCase : BaseHandler, IRequestHandler<UpdateAccessPointCommand, Result<Guid>>
    {
        private readonly IAccessPointsRepository _accessPointsRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateAccessPointUseCase(IAccessPointsRepository accessPointsRepository, IUnitOfWork unitOfWork)
        {
            _accessPointsRepository = accessPointsRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(UpdateAccessPointCommand command)
        {
            return await ExecuteAsync(async () =>
            {
                AccessPoint? accessPoint = await _accessPointsRepository.GetByIdAsync(command.Id);

                if (accessPoint is null)
                {
                    throw new NotFoundException($"No existe un punto de acceso con el ID {command.Id}.");
                }

                if (await _accessPointsRepository.ExistsByNameAsync(command.Name.Trim(), excludedAccessPointId: accessPoint.Id))
                {
                    throw new BusinessRuleException($"Ya existe otro punto de acceso con el nombre {command.Name}.");
                }

                accessPoint.Update(command.Name, command.Description, command.Type);

                if (command.IsEnabled)
                {
                    accessPoint.Enable();
                }
                else
                {
                    accessPoint.Disable();
                }

                await _accessPointsRepository.UpdateAsync(accessPoint);
                await _unitOfWork.CommitAsync();

                return accessPoint.Id;
            });
        }
    }
}
