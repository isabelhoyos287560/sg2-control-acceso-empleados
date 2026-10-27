using AccessControl.Domain.Entities.AccessPoints;
using AccessControl.Domain.Entities.AccessRecords;
using AccessControl.Domain.Entities.Employees;
using Microsoft.EntityFrameworkCore;

namespace AccessControl.Persistence
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Credential> Credentials { get; set; }
        public DbSet<AccessPoint> AccessPoints { get; set; }
        public DbSet<AccessRecord> AccessRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);

            base.OnModelCreating(builder);
        }
    }
}
