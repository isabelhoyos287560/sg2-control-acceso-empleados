using AccessControl.Application.UseCases.Credentials.Common;
using FluentValidation;

namespace AccessControl.Application.UseCases.Credentials.Commands.BlockCredential
{
    public sealed class BlockCredentialCommandValidator : AbstractValidator<BlockCredentialCommand>
    {
        public BlockCredentialCommandValidator()
        {
            RuleFor(cmd => cmd.Code).ValidCredentialCode();
        }
    }
}
