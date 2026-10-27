using AccessControl.Application.Contracts.Persistence;
using AccessControl.Application.Contracts.Repositories;
using AccessControl.Application.UseCases.Common;
using AccessControl.Application.Utilities.Mediator;
using AccessControl.Application.Utilities.Results;
using AccessControl.Domain.Entities.AccessPoints;
using AccessControl.Domain.Entities.AccessRecords;
using AccessControl.Domain.Entities.AccessRecords.ValueObjects;
using AccessControl.Domain.Entities.Employees;
using AccessControl.Domain.Entities.Employees.ValueObjects;

namespace AccessControl.Application.UseCases.AccessRecords.Commands.RegisterAccess
{
    public sealed class RegisterAccessUseCase : BaseHandler, IRequestHandler<RegisterAccessCommand, Result<AccessRecordResultDTO>>
    {
        private readonly IEmployeesRepository _employeesRepository;
        private readonly IAccessPointsRepository _accessPointsRepository;
        private readonly IAccessRecordsRepository _accessRecordsRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterAccessUseCase(IEmployeesRepository employeesRepository,
                                     IAccessPointsRepository accessPointsRepository,
                                     IAccessRecordsRepository accessRecordsRepository,
                                     IUnitOfWork unitOfWork)
        {
            _employeesRepository = employeesRepository;
            _accessPointsRepository = accessPointsRepository;
            _accessRecordsRepository = accessRecordsRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<AccessRecordResultDTO>> Handle(RegisterAccessCommand command)
        {
            return await ExecuteAsync(async () =>
            {
                CredentialCode code = CredentialCode.Create(command.CredentialCode);

                Employee? employee = await _employeesRepository.GetByCredentialCodeAsync(code);
                AccessPoint? accessPoint = await _accessPointsRepository.GetByIdAsync(command.AccessPointId);

                MovementType? lastAuthorizedMovement = employee is null
                    ? null
                    : await _accessRecordsRepository.GetLastAuthorizedMovementAsync(employee.Id);

                // Las reglas de autorización viven en el dominio; aquí solo se orquesta.
                // Un acceso rechazado también se persiste, para dejar trazabilidad.
                AccessRecord record = AccessRecord.Register(code,
                                                            employee,
                                                            accessPoint,
                                                            command.MovementType,
                                                            lastAuthorizedMovement);

                await _accessRecordsRepository.CreateAsync(record);
                await _unitOfWork.CommitAsync();

                return new AccessRecordResultDTO
                {
                    AccessRecordId = record.Id,
                    IsAuthorized = record.IsAuthorized,
                    Result = record.Result.ToString(),
                    RejectionReason = record.RejectionReason?.ToString(),
                    MovementType = record.MovementType.ToString(),
                    EmployeeId = record.EmployeeId,
                    AccessPointId = record.AccessPointId,
                    OccurredAt = record.OccurredAt,
                };
            });
        }
    }
}
