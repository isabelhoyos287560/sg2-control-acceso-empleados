using AccessControl.Domain.Common.ValueObjects;
using AccessControl.Domain.Entities.Employees.ValueObjects;
using AccessControl.Domain.Exceptions;

namespace AccessControl.Domain.Entities.Employees
{
    /// <summary>
    /// Raíz del agregado Employee. Las credenciales solo se crean y cambian de estado a través de él,
    /// para garantizar sus invariantes (empleado activo, una sola credencial vigente).
    /// </summary>
    public sealed class Employee
    {
        public Guid Id { get; private set; }
        public string IdentificationNumber { get; private set; } = null!;
        public string FirstName { get; private set; } = null!;
        public string LastName { get; private set; } = null!;
        public Email Email { get; private set; } = null!;
        public string Position { get; private set; } = null!;
        public OrganizationalArea Area { get; private set; } = null!;
        public EmploymentStatus Status { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? DeactivatedAt { get; private set; }

        private readonly List<Credential> _credentials = new();

        public IReadOnlyCollection<Credential> Credentials => _credentials;

        public string FullName => $"{FirstName} {LastName}";
        public bool IsActive => Status == EmploymentStatus.Active;

        private Employee()
        {

        }

        public Employee(string identificationNumber,
                        string firstName,
                        string lastName,
                        Email email,
                        string position,
                        OrganizationalArea area)
        {
            ApplyIdentificationNumberRules(identificationNumber);
            ApplyNameRules(firstName, "nombres");
            ApplyNameRules(lastName, "apellidos");
            ApplyEmailRules(email);
            ApplyPositionRules(position);
            ApplyAreaRules(area);

            Id = Guid.CreateVersion7();
            IdentificationNumber = identificationNumber.Trim();
            FirstName = firstName.Trim();
            LastName = lastName.Trim();
            Email = email;
            Position = position.Trim();
            Area = area;
            Status = EmploymentStatus.Active;
            CreatedAt = DateTime.UtcNow;
        }

        public void UpdateNames(string firstName, string lastName)
        {
            ApplyNameRules(firstName, "nombres");
            ApplyNameRules(lastName, "apellidos");

            FirstName = firstName.Trim();
            LastName = lastName.Trim();
        }

        public void UpdateEmail(Email email)
        {
            ApplyEmailRules(email);
            Email = email;
        }

        public void UpdatePosition(string position)
        {
            ApplyPositionRules(position);
            Position = position.Trim();
        }

        public void UpdateArea(OrganizationalArea area)
        {
            ApplyAreaRules(area);
            Area = area;
        }

        public void Deactivate()
        {
            if (Status == EmploymentStatus.Inactive)
            {
                throw new BusinessRuleException("El empleado ya se encuentra inactivo.");
            }

            Status = EmploymentStatus.Inactive;
            DeactivatedAt = DateTime.UtcNow;
        }

        public Credential AssignCredential(CredentialCode code)
        {
            if (code is null)
            {
                throw new BusinessRuleException("El código de la credencial es requerido.");
            }

            if (!IsActive)
            {
                throw new BusinessRuleException("No se pueden asignar credenciales a un empleado inactivo.");
            }

            if (_credentials.Any(c => !c.IsRevoked))
            {
                throw new BusinessRuleException("El empleado ya tiene una credencial vigente o bloqueada. Debe revocarla antes de asignar una nueva.");
            }

            if (_credentials.Any(c => c.Code.Equals(code)))
            {
                throw new BusinessRuleException("El empleado ya tuvo una credencial con ese código.");
            }

            Credential credential = new(Id, code);
            _credentials.Add(credential);

            return credential;
        }

        public Credential? FindCredential(CredentialCode code)
        {
            return _credentials.FirstOrDefault(c => c.Code.Equals(code));
        }

        public void BlockCredential(CredentialCode code)
        {
            GetCredentialOrThrow(code).Block();
        }

        public void UnblockCredential(CredentialCode code)
        {
            GetCredentialOrThrow(code).Unblock();
        }

        public void RevokeCredential(CredentialCode code)
        {
            GetCredentialOrThrow(code).Revoke();
        }

        private Credential GetCredentialOrThrow(CredentialCode code)
        {
            Credential? credential = FindCredential(code);

            if (credential is null)
            {
                throw new BusinessRuleException("La credencial no pertenece al empleado.");
            }

            return credential;
        }

        private static void ApplyIdentificationNumberRules(string identificationNumber)
        {
            if (string.IsNullOrWhiteSpace(identificationNumber))
            {
                throw new BusinessRuleException("La identificación del empleado es requerida.");
            }

            string trimmed = identificationNumber.Trim();

            if (trimmed.Length < 5 || trimmed.Length > 20)
            {
                throw new BusinessRuleException("La identificación debe tener entre 5 y 20 caracteres.");
            }

            if (!trimmed.All(char.IsLetterOrDigit))
            {
                throw new BusinessRuleException("La identificación solo admite letras y números.");
            }
        }

        private static void ApplyNameRules(string name, string field)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new BusinessRuleException($"Los {field} del empleado son requeridos.");
            }

            if (name.Trim().Length < 2 || name.Trim().Length > 64)
            {
                throw new BusinessRuleException($"Los {field} deben tener entre 2 y 64 caracteres.");
            }
        }

        private static void ApplyEmailRules(Email email)
        {
            if (email is null)
            {
                throw new BusinessRuleException("El correo electrónico es requerido.");
            }
        }

        private static void ApplyPositionRules(string position)
        {
            if (string.IsNullOrWhiteSpace(position))
            {
                throw new BusinessRuleException("El cargo del empleado es requerido.");
            }

            if (position.Trim().Length < 2 || position.Trim().Length > 64)
            {
                throw new BusinessRuleException("El cargo debe tener entre 2 y 64 caracteres.");
            }
        }

        private static void ApplyAreaRules(OrganizationalArea area)
        {
            if (area is null)
            {
                throw new BusinessRuleException("El área organizacional es requerida.");
            }
        }
    }
}
