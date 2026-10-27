namespace AccessControl.Application.UseCases.AccessRecords.Queries.GetAccessHistory
{
    public sealed class AccessRecordListItemDTO
    {
        public Guid Id { get; init; }
        public DateTime OccurredAt { get; init; }
        public string CredentialCode { get; init; } = null!;
        public Guid? EmployeeId { get; init; }
        public string? EmployeeFullName { get; init; }
        public Guid? AccessPointId { get; init; }
        public string? AccessPointName { get; init; }
        public string MovementType { get; init; } = null!;
        public string Result { get; init; } = null!;
        public string? RejectionReason { get; init; }
    }
}
