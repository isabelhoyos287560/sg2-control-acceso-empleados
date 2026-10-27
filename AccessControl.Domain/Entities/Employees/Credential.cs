using AccessControl.Domain.Entities.Employees.ValueObjects;
using AccessControl.Domain.Exceptions;

namespace AccessControl.Domain.Entities.Employees
{
    public sealed class Credential
    {
        public Guid Id { get; private set; }
        public Guid EmployeeId { get; private set; }
        public Employee Employee { get; private set; } = null!;
        public CredentialCode Code { get; private set; } = null!;
        public CredentialStatus Status { get; private set; }
        public DateTime IssuedAt { get; private set; }
        public DateTime? StatusChangedAt { get; private set; }

        public bool IsUsable => Status == CredentialStatus.Active;
        public bool IsRevoked => Status == CredentialStatus.Revoked;

        private Credential()
        {

        }

        // Solo el agregado Employee puede crear credenciales (ver Employee.AssignCredential)
        internal Credential(Guid employeeId, CredentialCode code)
        {
            ApplyEmployeeIdRules(employeeId);
            ApplyCodeRules(code);

            Id = Guid.CreateVersion7();
            EmployeeId = employeeId;
            Code = code;
            Status = CredentialStatus.Active;
            IssuedAt = DateTime.UtcNow;
        }

        internal void Block()
        {
            if (Status == CredentialStatus.Revoked)
            {
                throw new BusinessRuleException("No se puede bloquear una credencial revocada.");
            }

            if (Status == CredentialStatus.Blocked)
            {
                throw new BusinessRuleException("La credencial ya se encuentra bloqueada.");
            }

            Status = CredentialStatus.Blocked;
            StatusChangedAt = DateTime.UtcNow;
        }

        internal void Unblock()
        {
            if (Status != CredentialStatus.Blocked)
            {
                throw new BusinessRuleException("Solo se puede desbloquear una credencial bloqueada.");
            }

            Status = CredentialStatus.Active;
            StatusChangedAt = DateTime.UtcNow;
        }

        internal void Revoke()
        {
            if (Status == CredentialStatus.Revoked)
            {
                throw new BusinessRuleException("La credencial ya se encuentra revocada.");
            }

            Status = CredentialStatus.Revoked;
            StatusChangedAt = DateTime.UtcNow;
        }

        private static void ApplyEmployeeIdRules(Guid employeeId)
        {
            if (employeeId == Guid.Empty)
            {
                throw new BusinessRuleException("El empleado de la credencial es requerido.");
            }
        }

        private static void ApplyCodeRules(CredentialCode code)
        {
            if (code is null)
            {
                throw new BusinessRuleException("El código de la credencial es requerido.");
            }
        }
    }
}
