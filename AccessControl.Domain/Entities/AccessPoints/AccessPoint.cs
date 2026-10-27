using AccessControl.Domain.Entities.AccessPoints.ValueObjects;
using AccessControl.Domain.Exceptions;

namespace AccessControl.Domain.Entities.AccessPoints
{
    public sealed class AccessPoint
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = null!;
        public string? Description { get; private set; }
        public AccessPointType Type { get; private set; }
        public bool IsEnabled { get; private set; }
        public DateTime CreatedAt { get; private set; }

        private AccessPoint()
        {

        }

        public AccessPoint(string name, string? description, AccessPointType type, bool isEnabled = true)
        {
            ApplyNameRules(name);
            ApplyDescriptionRules(description);
            ApplyTypeRules(type);

            Id = Guid.CreateVersion7();
            Name = name.Trim();
            Description = description?.Trim();
            Type = type;
            IsEnabled = isEnabled;
            CreatedAt = DateTime.UtcNow;
        }

        public void Update(string name, string? description, AccessPointType type)
        {
            ApplyNameRules(name);
            ApplyDescriptionRules(description);
            ApplyTypeRules(type);

            Name = name.Trim();
            Description = description?.Trim();
            Type = type;
        }

        public void Enable()
        {
            IsEnabled = true;
        }

        public void Disable()
        {
            IsEnabled = false;
        }

        private static void ApplyNameRules(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new BusinessRuleException("El nombre del punto de acceso es requerido.");
            }

            if (name.Trim().Length < 3 || name.Trim().Length > 64)
            {
                throw new BusinessRuleException("El nombre del punto de acceso debe tener entre 3 y 64 caracteres.");
            }
        }

        private static void ApplyDescriptionRules(string? description)
        {
            if (description is not null && description.Trim().Length > 256)
            {
                throw new BusinessRuleException("La descripción del punto de acceso debe tener máximo 256 caracteres.");
            }
        }

        private static void ApplyTypeRules(AccessPointType type)
        {
            if (!Enum.IsDefined(type))
            {
                throw new BusinessRuleException("El tipo de punto de acceso no es válido.");
            }
        }
    }
}
