using AccessControl.Domain.Exceptions;

namespace AccessControl.Domain.Entities.Employees.ValueObjects
{
    public sealed record OrganizationalArea
    {
        public string Code { get; }
        public string Name { get; }

        public static readonly OrganizationalArea Administration = new("ADMINISTRATION", "Administración");
        public static readonly OrganizationalArea Operations = new("OPERATIONS", "Operaciones");
        public static readonly OrganizationalArea Warehouse = new("WAREHOUSE", "Almacenamiento");
        public static readonly OrganizationalArea Logistics = new("LOGISTICS", "Logística");
        public static readonly OrganizationalArea Security = new("SECURITY", "Seguridad");
        public static readonly OrganizationalArea HumanResources = new("HUMAN_RESOURCES", "Talento Humano");
        public static readonly OrganizationalArea Technology = new("TECHNOLOGY", "Tecnología");

        private static readonly IReadOnlyCollection<OrganizationalArea> _all = new List<OrganizationalArea>
        {
            Administration,
            Operations,
            Warehouse,
            Logistics,
            Security,
            HumanResources,
            Technology
        }.AsReadOnly();

        public static IReadOnlyCollection<OrganizationalArea> All => _all;

        private OrganizationalArea(string code, string name)
        {
            Code = code;
            Name = name;
        }

        public static OrganizationalArea FromCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new BusinessRuleException("El código del área organizacional es requerido.");
            }

            string normalizedCode = code.Trim()
                                        .Replace(" ", "_")
                                        .ToUpper();

            OrganizationalArea? area = _all.FirstOrDefault(a => a.Code == normalizedCode);

            if (area is null)
            {
                throw new BusinessRuleException("No existe un área organizacional con el código proporcionado.");
            }

            return area;
        }

        public static bool IsSupported(string? code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                return false;
            }

            string normalizedCode = code.Trim()
                                        .Replace(" ", "_")
                                        .ToUpper();

            return _all.Any(a => a.Code == normalizedCode);
        }

        public bool Equals(OrganizationalArea? other)
        {
            if (other is null)
            {
                return false;
            }

            return Code == other.Code;
        }

        public override int GetHashCode() => Code.GetHashCode();

        public override string ToString() => Name;
    }
}
