using AccessControl.Domain.Entities.Employees.ValueObjects;
using FluentValidation;

namespace AccessControl.Application.UseCases.Credentials.Common
{
    internal static class CredentialCodeRules
    {
        public static IRuleBuilderOptions<T, string> ValidCredentialCode<T>(this IRuleBuilder<T, string> rule)
        {
            return rule.NotEmpty().WithMessage("El código de la credencial es requerido.")
                       .Length(CredentialCode.MIN_LENGTH, CredentialCode.MAX_LENGTH)
                       .WithMessage($"El código de la credencial debe tener entre {CredentialCode.MIN_LENGTH} y {CredentialCode.MAX_LENGTH} caracteres.")
                       .Matches("^[a-zA-Z0-9-]+$").WithMessage("El código de la credencial solo admite letras, números y guiones.");
        }
    }
}
