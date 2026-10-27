namespace AccessControl.Application.UseCases.Credentials.Queries.GetCredentialByCode
{
    public sealed class CredentialDetailsDTO
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = null!;
        public string Status { get; set; } = null!;

        /// <summary>
        /// Indica si la credencial puede utilizarse en este momento (credencial activa y empleado activo).
        /// </summary>
        public bool CanBeUsed { get; set; }

        public DateTime IssuedAt { get; set; }
        public DateTime? StatusChangedAt { get; set; }
        public Guid EmployeeId { get; set; }
        public string EmployeeFullName { get; set; } = null!;
        public string EmployeeStatus { get; set; } = null!;
    }
}
