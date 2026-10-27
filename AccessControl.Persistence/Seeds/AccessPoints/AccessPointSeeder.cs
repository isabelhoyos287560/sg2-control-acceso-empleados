using AccessControl.Domain.Entities.AccessPoints;
using AccessControl.Domain.Entities.AccessPoints.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace AccessControl.Persistence.Seeds.AccessPoints
{
    public class AccessPointSeeder : IDataSeeder
    {
        private readonly DataContext _context;

        public AccessPointSeeder(DataContext context)
        {
            _context = context;
        }

        public int Order => 1;

        public async Task SeedAsync(CancellationToken cancellationToken = default)
        {
            if (await _context.AccessPoints.AnyAsync(cancellationToken))
            {
                return;
            }

            List<AccessPoint> accessPoints =
            [
                new AccessPoint("Recepción principal", "Ingreso peatonal de la sede principal", AccessPointType.Reception),
                new AccessPoint("Parqueadero", "Ingreso vehicular de empleados", AccessPointType.Parking),
                new AccessPoint("Bodega central", "Zona de almacenamiento de mercancía", AccessPointType.Warehouse),
                new AccessPoint("Cuarto de servidores", "Área restringida de tecnología", AccessPointType.RestrictedArea),
                new AccessPoint("Bodega norte", "Fuera de servicio por mantenimiento", AccessPointType.Warehouse, isEnabled: false),
            ];

            await _context.AccessPoints.AddRangeAsync(accessPoints, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
