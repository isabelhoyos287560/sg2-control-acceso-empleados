namespace AccessControl.Application.UseCases.AccessRecords.Commands.RegisterAccess
{
    public sealed class AccessRecordResultDTO
    {
        public Guid AccessRecordId { get; init; }
        public bool IsAuthorized { get; init; }
        public string Result { get; init; } = null!;
        public string? RejectionReason { get; init; }
        public string MovementType { get; init; } = null!;
        public Guid? EmployeeId { get; init; }
        public Guid? AccessPointId { get; init; }
        public DateTime OccurredAt { get; init; }
    }
}
