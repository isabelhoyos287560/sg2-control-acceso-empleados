using AccessControl.Domain.Entities.AccessRecords;

namespace AccessControl.Application.UseCases.AccessRecords.Queries.GetAccessHistory
{
    internal static class MapperExtensions
    {
        public static AccessRecordListItemDTO ToListItemDTO(this AccessRecord record)
        {
            return new AccessRecordListItemDTO
            {
                Id = record.Id,
                OccurredAt = record.OccurredAt,
                CredentialCode = record.CredentialCode,
                EmployeeId = record.EmployeeId,
                EmployeeFullName = record.Employee?.FullName,
                AccessPointId = record.AccessPointId,
                AccessPointName = record.AccessPoint?.Name,
                MovementType = record.MovementType.ToString(),
                Result = record.Result.ToString(),
                RejectionReason = record.RejectionReason?.ToString(),
            };
        }
    }
}
