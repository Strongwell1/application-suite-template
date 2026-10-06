using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Skeleton.Core.Domain.Widgets;
using Skeleton.Core.Domain.Shared;

namespace Skeleton.Core.Persistence.Configurations;

public class WidgetConfiguration : IEntityTypeConfiguration<Widget>
{
    public void Configure(EntityTypeBuilder<Widget> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(ModelConstraints.MaxStringLengthDefault);

        builder.Property(x => x.Description)
            .HasMaxLength(ModelConstraints.MaxDescriptionLength);

        builder.Property(x => x.State)
            .HasConversion<int>()
            .HasDefaultValue(WidgetState.Active)
            .HasSentinel(0)
            .IsRequired();

        builder.Property(x => x.UpdatedUtc);

        builder.Property(x => x.DeletedReason)
            .HasMaxLength(ModelConstraints.MaxStringLengthDefault);

        builder.Property(x => x.DeletedUtc);

        // Example owned entity configuration for Actor value object
        builder.OwnsOne(x => x.CreatedBy, actor =>
        {
            actor.Property(a => a.Id).HasColumnName("CreatedById");
            actor.Property(a => a.DisplayName).HasColumnName("CreatedByDisplayName")
                .HasMaxLength(ModelConstraints.MaxStringLengthDefault);
            actor.Property(a => a.Email).HasColumnName("CreatedByEmail")
                .HasMaxLength(ModelConstraints.MaxEmailLength);
        });

        // Same owned-entity pattern, reused for the delete audit trail
        builder.OwnsOne(x => x.DeletedBy, actor =>
        {
            actor.Property(a => a.Id).HasColumnName("DeletedById");
            actor.Property(a => a.DisplayName).HasColumnName("DeletedByDisplayName")
                .HasMaxLength(ModelConstraints.MaxStringLengthDefault);
            actor.Property(a => a.Email).HasColumnName("DeletedByEmail")
                .HasMaxLength(ModelConstraints.MaxEmailLength);
        });
    }
}
