using AccessControl.Application.Utilities.Mediator;
using AccessControl.Application.Utilities.Results;
using AccessControl.Domain.Entities.AccessRecords.ValueObjects;

namespace AccessControl.Application.UseCases.AccessRecords.Commands.RegisterAccess
{
    public sealed class RegisterAccessCommand : IRequest<Result<AccessRecordResultDTO>>
    {
        public string CredentialCode { get; set; } = string.Empty;
        public Guid AccessPointId { get; set; }
        public MovementType MovementType { get; set; }
    }
}
