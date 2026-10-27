using AccessControl.Application.UseCases.Credentials.Common;
using FluentValidation;

namespace AccessControl.Application.UseCases.Credentials.Commands.RevokeCredential
{
    public sealed class RevokeCredentialCommandValidator : AbstractValidator<RevokeCredentialCommand>
    {
        public RevokeCredentialCommandValidator()
        {
            RuleFor(cmd => cmd.Code).ValidCredentialCode();
        }
    }
}
