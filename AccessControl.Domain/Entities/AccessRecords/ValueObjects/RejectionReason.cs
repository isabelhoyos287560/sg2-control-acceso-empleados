namespace AccessControl.Domain.Entities.AccessRecords.ValueObjects
{
    public enum RejectionReason
    {
        AccessPointNotFound = 1,
        AccessPointDisabled = 2,
        CredentialNotFound = 3,
        CredentialBlocked = 4,
        CredentialRevoked = 5,
        EmployeeInactive = 6,
        EmployeeAlreadyInside = 7,
        EmployeeNotInside = 8,
    }
}
