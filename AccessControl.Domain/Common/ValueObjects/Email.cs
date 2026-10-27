using AccessControl.Domain.Exceptions;
using System.Text.RegularExpressions;

namespace AccessControl.Domain.Common.ValueObjects
{
    public sealed record Email
    {
        private const int MAX_LENGTH = 128;
        private static readonly Regex _pattern = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

        public string Value { get; }

        private Email(string value)
        {
            Value = value;
        }

        public static Email Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new BusinessRuleException("El correo electrónico es requerido.");
            }

            string normalized = value.Trim()
                                     .ToLowerInvariant();

            if (normalized.Length > MAX_LENGTH)
            {
                throw new BusinessRuleException($"El correo electrónico no puede exceder {MAX_LENGTH} caracteres.");
            }

            if (!_pattern.IsMatch(normalized))
            {
                throw new BusinessRuleException("El correo electrónico no tiene un formato válido.");
            }

            return new Email(normalized);
        }

        public override string ToString() => Value;
    }
}
