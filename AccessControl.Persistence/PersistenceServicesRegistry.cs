using AccessControl.Application.Contracts.Persistence;
using AccessControl.Application.Contracts.Repositories;
using AccessControl.Persistence.Repositories;
using AccessControl.Persistence.Seeds;
using AccessControl.Persistence.Seeds.AccessPoints;
using AccessControl.Persistence.Seeds.Employees;
using AccessControl.Persistence.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AccessControl.Persistence
{
    public static class PersistenceServicesRegistry
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("MyConnection"));
            });

            services.AddScoped<IUnitOfWork, EfCoreUnitOfWork>();
            services.AddScoped<IEmployeesRepository, EmployeesRepository>();
            services.AddScoped<IAccessPointsRepository, AccessPointsRepository>();
            services.AddScoped<IAccessRecordsRepository, AccessRecordsRepository>();

            // Seeders
            services.AddScoped<IDataSeeder, AccessPointSeeder>();
            services.AddScoped<IDataSeeder, EmployeeSeeder>();

            return services;
        }
    }
}
