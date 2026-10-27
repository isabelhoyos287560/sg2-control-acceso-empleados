using AccessControl.Application.Utilities.Mediator;
using AccessControl.Application.Utilities.Results;
using AccessControl.Domain.Entities.AccessPoints.ValueObjects;

namespace AccessControl.Application.UseCases.AccessPoints.Commands.CreateAccessPoint
{
    public sealed class CreateAccessPointCommand : IRequest<Result<Guid>>
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public AccessPointType Type { get; set; }
        public bool IsEnabled { get; set; } = true;
    }
}
