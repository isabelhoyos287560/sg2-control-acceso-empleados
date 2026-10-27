using AccessControl.Domain.Common.ValueObjects;
using AccessControl.Domain.Entities.Employees;
using AccessControl.Domain.Entities.Employees.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccessControl.Persistence.Configurations
{
    internal class EmployeeConfig : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasKey(e => e.Id);

            // El Id se genera en el dominio (Guid v7), no en la base de datos
            builder.Property(e => e.Id)
                   .ValueGeneratedNever();

            builder.Property(e => e.IdentificationNumber)
                   .HasMaxLength(20)
                   .IsRequired();

            builder.HasIndex(e => e.IdentificationNumber)
                   .IsUnique();

            builder.Property(e => e.FirstName)
                   .HasMaxLength(64)
                   .IsRequired();

            builder.Property(e => e.LastName)
                   .HasMaxLength(64)
                   .IsRequired();

            builder.Property(e => e.Email)
                   .HasConversion(email => email.Value, value => Email.Create(value))
                   .HasMaxLength(128)
                   .IsRequired();

            builder.HasIndex(e => e.Email)
                   .IsUnique();

            builder.Property(e => e.Position)
                   .HasMaxLength(64)
                   .IsRequired();

            builder.Property(e => e.Area)
                   .HasColumnName("AreaCode")
                   .HasConversion(area => area.Code, code => OrganizationalArea.FromCode(code))
                   .HasMaxLength(32)
                   .IsRequired();

            builder.Property(e => e.Status)
                   .IsRequired();

            builder.Property(e => e.CreatedAt)
                   .IsRequired();

            builder.Ignore(e => e.FullName);
            builder.Ignore(e => e.IsActive);

            builder.HasMany(e => e.Credentials)
                   .WithOne(c => c.Employee)
                   .HasForeignKey(c => c.EmployeeId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Navigation(e => e.Credentials)
                   .HasField("_credentials")
                   .UsePropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
