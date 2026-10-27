using AccessControl.Application.Utilities.Mediator;
using AccessControl.Application.Utilities.Results;

namespace AccessControl.Application.UseCases.Credentials.Commands.UnblockCredential
{
    public sealed class UnblockCredentialCommand : IRequest<Result<Guid>>
    {
        public string Code { get; set; } = string.Empty;
    }
}
