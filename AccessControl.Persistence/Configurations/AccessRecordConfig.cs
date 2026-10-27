using AccessControl.Domain.Entities.AccessRecords;
using AccessControl.Domain.Entities.Employees.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccessControl.Persistence.Configurations
{
    internal class AccessRecordConfig : IEntityTypeConfiguration<AccessRecord>
    {
        public void Configure(EntityTypeBuilder<AccessRecord> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.Id)
                   .ValueGeneratedNever();

            builder.Property(r => r.CredentialCode)
                   .HasMaxLength(CredentialCode.MAX_LENGTH)
                   .IsRequired();

            builder.Property(r => r.MovementType)
                   .IsRequired();

            builder.Property(r => r.Result)
                   .IsRequired();

            builder.Property(r => r.RejectionReason);

            builder.Property(r => r.OccurredAt)
                   .IsRequired();

            builder.Ignore(r => r.IsAuthorized);

            builder.HasOne(r => r.Employee)
                   .WithMany()
                   .HasForeignKey(r => r.EmployeeId)
                   .IsRequired(false)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(r => r.AccessPoint)
                   .WithMany()
                   .HasForeignKey(r => r.AccessPointId)
                   .IsRequired(false)
                   .OnDelete(DeleteBehavior.Restrict);

            // Índices para las consultas del historial (RF-04)
            builder.HasIndex(r => new { r.EmployeeId, r.OccurredAt });
            builder.HasIndex(r => new { r.AccessPointId, r.OccurredAt });
            builder.HasIndex(r => r.OccurredAt);
        }
    }
}
