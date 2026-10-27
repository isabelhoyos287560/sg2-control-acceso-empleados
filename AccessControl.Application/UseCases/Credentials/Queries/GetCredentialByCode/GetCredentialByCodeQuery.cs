using AccessControl.Application.Utilities.Mediator;
using AccessControl.Application.Utilities.Results;

namespace AccessControl.Application.UseCases.Credentials.Queries.GetCredentialByCode
{
    public sealed class GetCredentialByCodeQuery : IRequest<Result<CredentialDetailsDTO>>
    {
        public string Code { get; set; } = string.Empty;
    }
}
