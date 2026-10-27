using AccessControl.Domain.Entities.AccessPoints;
using AccessControl.Domain.Entities.AccessRecords.ValueObjects;
using AccessControl.Domain.Entities.Employees;
using AccessControl.Domain.Entities.Employees.ValueObjects;
using AccessControl.Domain.Exceptions;

namespace AccessControl.Domain.Entities.AccessRecords
{
    /// <summary>
    /// Movimiento de entrada o salida. Se registra siempre, sea autorizado o rechazado,
    /// para tener trazabilidad completa de los intentos de acceso.
    /// </summary>
    public sealed class AccessRecord
    {
        public Guid Id { get; private set; }
        public string CredentialCode { get; private set; } = null!;
        public Guid? EmployeeId { get; private set; }
        public Employee? Employee { get; private set; }
        public Guid? AccessPointId { get; private set; }
        public AccessPoint? AccessPoint { get; private set; }
        public MovementType MovementType { get; private set; }
        public AccessResult Result { get; private set; }
        public RejectionReason? RejectionReason { get; private set; }
        public DateTime OccurredAt { get; private set; }

        public bool IsAuthorized => Result == AccessResult.Authorized;

        private AccessRecord()
        {

        }

        private AccessRecord(CredentialCode credentialCode,
                             Guid? employeeId,
                             Guid? accessPointId,
                             MovementType movementType,
                             RejectionReason? rejectionReason)
        {
            Id = Guid.CreateVersion7();
            CredentialCode = credentialCode.Value;
            EmployeeId = employeeId;
            AccessPointId = accessPointId;
            MovementType = movementType;
            Result = rejectionReason is null ? AccessResult.Authorized : AccessResult.Rejected;
            RejectionReason = rejectionReason;
            OccurredAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Evalúa las reglas de acceso y produce el registro correspondiente (autorizado o rechazado).
        /// </summary>
        /// <param name="credentialCode">Código de credencial presentado.</param>
        /// <param name="employee">Empleado dueño de la credencial, o null si la credencial no existe.</param>
        /// <param name="accessPoint">Punto de acceso, o null si no existe.</param>
        /// <param name="movementType">Entrada o salida.</param>
        /// <param name="lastAuthorizedMovement">Último movimiento autorizado del empleado, o null si no tiene.</param>
        public static AccessRecord Register(CredentialCode credentialCode,
                                            Employee? employee,
                                            AccessPoint? accessPoint,
                                            MovementType movementType,
                                            MovementType? lastAuthorizedMovement)
        {
            if (credentialCode is null)
            {
                throw new BusinessRuleException("El código de la credencial es requerido.");
            }

            if (!Enum.IsDefined(movementType))
            {
                throw new BusinessRuleException("El tipo de movimiento no es válido.");
            }

            Credential? credential = employee?.FindCredential(credentialCode);

            RejectionReason? reason = Evaluate(credential, employee, accessPoint, movementType, lastAuthorizedMovement);

            return new AccessRecord(credentialCode,
                                    employee?.Id,
                                    accessPoint?.Id,
                                    movementType,
                                    reason);
        }

        private static RejectionReason? Evaluate(Credential? credential,
                                                 Employee? employee,
                                                 AccessPoint? accessPoint,
                                                 MovementType movementType,
                                                 MovementType? lastAuthorizedMovement)
        {
            if (accessPoint is null)
            {
                return ValueObjects.RejectionReason.AccessPointNotFound;
            }

            if (!accessPoint.IsEnabled)
            {
                return ValueObjects.RejectionReason.AccessPointDisabled;
            }

            if (credential is null || employee is null)
            {
                return ValueObjects.RejectionReason.CredentialNotFound;
            }

            if (credential.Status == CredentialStatus.Blocked)
            {
                return ValueObjects.RejectionReason.CredentialBlocked;
            }

            if (credential.Status == CredentialStatus.Revoked)
            {
                return ValueObjects.RejectionReason.CredentialRevoked;
            }

            if (!employee.IsActive)
            {
                return ValueObjects.RejectionReason.EmployeeInactive;
            }

            // Evita registros duplicados: no se puede entrar dos veces sin salir, ni salir sin haber entrado
            if (movementType == MovementType.Entry && lastAuthorizedMovement == MovementType.Entry)
            {
                return ValueObjects.RejectionReason.EmployeeAlreadyInside;
            }

            if (movementType == MovementType.Exit && lastAuthorizedMovement != MovementType.Entry)
            {
                return ValueObjects.RejectionReason.EmployeeNotInside;
            }

            return null;
        }
    }
}
