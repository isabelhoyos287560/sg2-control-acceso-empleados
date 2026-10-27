using AccessControl.Domain.Entities.AccessPoints;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccessControl.Persistence.Configurations
{
    internal class AccessPointConfig : IEntityTypeConfiguration<AccessPoint>
    {
        public void Configure(EntityTypeBuilder<AccessPoint> builder)
        {
            builder.HasKey(ap => ap.Id);

            builder.Property(ap => ap.Id)
                   .ValueGeneratedNever();

            builder.Property(ap => ap.Name)
                   .HasMaxLength(64)
                   .IsRequired();

            builder.HasIndex(ap => ap.Name)
                   .IsUnique();

            builder.Property(ap => ap.Description)
                   .HasMaxLength(256);

            builder.Property(ap => ap.Type)
                   .IsRequired();

            builder.Property(ap => ap.IsEnabled)
                   .IsRequired();

            builder.Property(ap => ap.CreatedAt)
                   .IsRequired();
        }
    }
}
