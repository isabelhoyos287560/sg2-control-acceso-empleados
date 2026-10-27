using AccessControl.Domain.Entities.Employees;
using AccessControl.Domain.Entities.Employees.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccessControl.Persistence.Configurations
{
    internal class CredentialConfig : IEntityTypeConfiguration<Credential>
    {
        public void Configure(EntityTypeBuilder<Credential> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                   .ValueGeneratedNever();

            builder.Property(c => c.Code)
                   .HasConversion(code => code.Value, value => CredentialCode.Create(value))
                   .HasMaxLength(CredentialCode.MAX_LENGTH)
                   .IsRequired();

            // Código único de credencial en toda la organización
            builder.HasIndex(c => c.Code)
                   .IsUnique();

            builder.Property(c => c.Status)
                   .IsRequired();

            builder.Property(c => c.IssuedAt)
                   .IsRequired();

            builder.Ignore(c => c.IsUsable);
            builder.Ignore(c => c.IsRevoked);
        }
    }
}
