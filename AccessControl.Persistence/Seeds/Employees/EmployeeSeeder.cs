using AccessControl.Domain.Common.ValueObjects;
using AccessControl.Domain.Entities.Employees;
using AccessControl.Domain.Entities.Employees.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace AccessControl.Persistence.Seeds.Employees
{
    /// <summary>
    /// Datos de prueba para evidenciar los casos de uso: un empleado activo con credencial vigente,
    /// uno con credencial bloqueada y uno inactivo.
    /// </summary>
    public class EmployeeSeeder : IDataSeeder
    {
        private readonly DataContext _context;

        public EmployeeSeeder(DataContext context)
        {
            _context = context;
        }

        public int Order => 2;

        public async Task SeedAsync(CancellationToken cancellationToken = default)
        {
            if (await _context.Employees.AnyAsync(cancellationToken))
            {
                return;
            }

            Employee ana = new("1037000001", "Ana", "Gómez", Email.Create("ana.gomez@logistica.com"), "Auxiliar de bodega", OrganizationalArea.Warehouse);
            ana.AssignCredential(CredentialCode.Create("CRED-0001"));

            Employee carlos = new("1037000002", "Carlos", "Pérez", Email.Create("carlos.perez@logistica.com"), "Analista de inventarios", OrganizationalArea.Logistics);
            carlos.AssignCredential(CredentialCode.Create("CRED-0002"));
            carlos.BlockCredential(CredentialCode.Create("CRED-0002"));

            Employee laura = new("1037000003", "Laura", "Restrepo", Email.Create("laura.restrepo@logistica.com"), "Coordinadora de seguridad", OrganizationalArea.Security);
            laura.AssignCredential(CredentialCode.Create("CRED-0003"));
            laura.Deactivate();

            await _context.Employees.AddRangeAsync([ana, carlos, laura], cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
