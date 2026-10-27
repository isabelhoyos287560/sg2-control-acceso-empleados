using AccessControl.Application.UseCases.Credentials.Common;
using FluentValidation;

namespace AccessControl.Application.UseCases.Credentials.Commands.UnblockCredential
{
    public sealed class UnblockCredentialCommandValidator : AbstractValidator<UnblockCredentialCommand>
    {
        public UnblockCredentialCommandValidator()
        {
            RuleFor(cmd => cmd.Code).ValidCredentialCode();
        }
    }
}
