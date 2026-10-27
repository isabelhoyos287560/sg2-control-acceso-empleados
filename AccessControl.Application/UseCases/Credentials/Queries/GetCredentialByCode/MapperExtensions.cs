using AccessControl.Domain.Entities.Employees;

namespace AccessControl.Application.UseCases.Credentials.Queries.GetCredentialByCode
{
    internal static class MapperExtensions
    {
        public static CredentialDetailsDTO ToDetailsDTO(this Credential credential, Employee employee)
        {
            return new CredentialDetailsDTO
            {
                Id = credential.Id,
                Code = credential.Code.Value,
                Status = credential.Status.ToString(),
                CanBeUsed = credential.IsUsable && employee.IsActive,
                IssuedAt = credential.IssuedAt,
                StatusChangedAt = credential.StatusChangedAt,
                EmployeeId = employee.Id,
                EmployeeFullName = employee.FullName,
                EmployeeStatus = employee.Status.ToString(),
            };
        }
    }
}
