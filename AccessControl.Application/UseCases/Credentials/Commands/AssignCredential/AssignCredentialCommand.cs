using AccessControl.Application.Utilities.Mediator;
using AccessControl.Application.Utilities.Results;

namespace AccessControl.Application.UseCases.Credentials.Commands.AssignCredential
{
    public sealed class AssignCredentialCommand : IRequest<Result<Guid>>
    {
        public Guid EmployeeId { get; set; }
        public string Code { get; set; } = string.Empty;
    }
}
