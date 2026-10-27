using AccessControl.Domain.Entities.AccessPoints;

namespace AccessControl.Application.UseCases.AccessPoints.Queries.GetAccessPointsList
{
    public static class MapperExtensions
    {
        public static AccessPointDTO ToDTO(this AccessPoint accessPoint)
        {
            return new AccessPointDTO
            {
                Id = accessPoint.Id,
                Name = accessPoint.Name,
                Description = accessPoint.Description,
                Type = accessPoint.Type.ToString(),
                IsEnabled = accessPoint.IsEnabled,
                CreatedAt = accessPoint.CreatedAt,
            };
        }
    }
}
