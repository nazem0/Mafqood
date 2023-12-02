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
               .Property(r => r.Name)
                .HasMaxLength(200);


            builder
                .Property(r => r.Street)
                .HasMaxLength(255);

            builder
                .Property(r => r.District)
                .HasMaxLength(100);

            builder
                .Property(r => r.ContactNumber)
                .HasMaxLength(19);

            builder
                .Property(r => r.AdditionalInfo)
                .HasMaxLength(1500);

            builder
                .Property(r => r.DeletionCode)
                .HasMaxLength(32);

            builder
                .HasMany(r => r.Attachments)
                .WithOne(a => a.Report)
                .HasForeignKey(a => a.ReportId);
        }
    }
}
