using AccessControl.Application.Utilities.Mediator;
using AccessControl.Application.Utilities.Results;

namespace AccessControl.Application.UseCases.Credentials.Commands.RevokeCredential
{
    public sealed class RevokeCredentialCommand : IRequest<Result<Guid>>
    {
        public string Code { get; set; } = string.Empty;
    }
}
