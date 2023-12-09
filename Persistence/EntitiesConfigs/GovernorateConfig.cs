using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntitiesConfigs
{
    public class GovernorateConfig : IEntityTypeConfiguration<Governorate>
    {
        public void Configure(EntityTypeBuilder<Governorate> builder)
        {
            builder
                .ToTable("Governorate")
                .HasKey(g => g.Id);

            builder
                .Property(g => g.Id)
                .ValueGeneratedOnAdd();

            builder
                .HasMany(g => g.Cities)
                .WithOne(c => c.Governorate)
                .HasForeignKey(c => c.GovernorateId);

        }
    }
}
