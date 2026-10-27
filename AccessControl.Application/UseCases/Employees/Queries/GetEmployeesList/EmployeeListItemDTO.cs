namespace AccessControl.Application.UseCases.Employees.Queries.GetEmployeesList
{
    public sealed class EmployeeListItemDTO
    {
        public Guid Id { get; init; }
        public string IdentificationNumber { get; init; } = null!;
        public string FullName { get; init; } = null!;
        public string Email { get; init; } = null!;
        public string Position { get; init; } = null!;
        public string AreaCode { get; init; } = null!;
        public string AreaName { get; init; } = null!;
        public string Status { get; init; } = null!;
    }
}
