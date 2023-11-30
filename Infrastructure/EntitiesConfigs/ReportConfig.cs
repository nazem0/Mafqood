using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configs
{
    public class ReportConfig : IEntityTypeConfiguration<Report>
    {
        public void Configure(EntityTypeBuilder<Report> builder)
        {
            builder
                .ToTable("Report")
                .HasKey(r => r.Id);

            builder
                .Property(r => r.Id)
                .ValueGeneratedOnAdd();

            builder
               .Property(u => u.Name)
                .HasMaxLength(200);

            builder
                .Property(u => u.Street)
                .HasMaxLength(255);

            builder
                .Property(u => u.District)
                .HasMaxLength(100)
                .IsRequired();

            builder
                .Property(u => u.ContactNumber)
                .HasMaxLength(19)
                .IsRequired();

            builder
                .Property(u => u.AdditionalInfo)
                .HasMaxLength(1500);

            builder
                .HasMany(r => r.Attachments)
                .WithOne(a => a.Report)
                .HasForeignKey(a => a.ReportId);
        }
    }
}
