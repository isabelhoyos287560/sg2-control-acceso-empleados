namespace AccessControl.Application.UseCases.AccessPoints.Queries.GetAccessPointsList
{
    public sealed class AccessPointDTO
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = null!;
        public string? Description { get; init; }
        public string Type { get; init; } = null!;
        public bool IsEnabled { get; init; }
        public DateTime CreatedAt { get; init; }
    }
}
