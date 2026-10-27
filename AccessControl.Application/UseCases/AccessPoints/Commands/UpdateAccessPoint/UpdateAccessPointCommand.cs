using AccessControl.Application.Utilities.Mediator;
using AccessControl.Application.Utilities.Results;
using AccessControl.Domain.Entities.AccessPoints.ValueObjects;

namespace AccessControl.Application.UseCases.AccessPoints.Commands.UpdateAccessPoint
{
    public sealed class UpdateAccessPointCommand : IRequest<Result<Guid>>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public AccessPointType Type { get; set; }
        public bool IsEnabled { get; set; }
    }
}
