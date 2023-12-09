using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntitiesConfigs
{
    public class AttachmentConfig : IEntityTypeConfiguration<Attachment>
    {
        public void Configure(EntityTypeBuilder<Attachment> builder)
        {
            builder
                .ToTable("Attachment")
                .HasKey(a => a.Id);

            builder
                .Property(a => a.Name)
                .HasMaxLength(256)
                .IsRequired();
        }
    }
}
