using AccessControl.Domain.Exceptions;

namespace AccessControl.Domain.Entities.Employees.ValueObjects
{
    public sealed record CredentialCode
    {
        public const int MIN_LENGTH = 6;
        public const int MAX_LENGTH = 32;

        public string Value { get; }

        private CredentialCode(string value)
        {
            Value = value;
        }

        public static CredentialCode Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new BusinessRuleException("El código de la credencial es requerido.");
            }

            string normalized = Normalize(value);

            if (normalized.Length < MIN_LENGTH || normalized.Length > MAX_LENGTH)
            {
                throw new BusinessRuleException($"El código de la credencial debe tener entre {MIN_LENGTH} y {MAX_LENGTH} caracteres.");
            }

            if (!normalized.All(c => char.IsLetterOrDigit(c) || c == '-'))
            {
                throw new BusinessRuleException("El código de la credencial solo admite letras, números y guiones.");
            }

            return new CredentialCode(normalized);
        }

        public static string Normalize(string value) => value.Trim().ToUpperInvariant();

        public override string ToString() => Value;
    }
}
