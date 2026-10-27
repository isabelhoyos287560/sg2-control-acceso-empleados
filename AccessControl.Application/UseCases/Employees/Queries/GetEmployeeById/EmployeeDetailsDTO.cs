namespace AccessControl.Application.UseCases.Employees.Queries.GetEmployeeById
{
    public sealed class EmployeeDetailsDTO
    {
        public Guid Id { get; set; }
        public string IdentificationNumber { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Position { get; set; } = null!;
        public string AreaCode { get; set; } = null!;
        public string AreaName { get; set; } = null!;
        public string Status { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime? DeactivatedAt { get; set; }
        public IReadOnlyCollection<EmployeeCredentialDTO> Credentials { get; set; } = [];
    }

    public sealed class EmployeeCredentialDTO
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = null!;
        public string Status { get; set; } = null!;
        public DateTime IssuedAt { get; set; }
        public DateTime? StatusChangedAt { get; set; }
    }
}
