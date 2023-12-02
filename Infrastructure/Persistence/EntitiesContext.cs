using Domain.Entities;
using Infrastructure.DataSeeding;
using Infrastructure.Persistence.EntitiesConfigs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class EntitiesContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public EntitiesContext() { }
        public EntitiesContext(DbContextOptions options) : base(options) { }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<Governorate> Governorates { get; set; }
        public DbSet<City> Cities { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .ApplyConfiguration(new ReportConfig())
                .ApplyConfiguration(new AttachmentConfig())
                .ApplyConfiguration(new GovernorateConfig())
                .ApplyConfiguration(new CityConfig())
                .SeedRoles()
                .SeedUsers()
                .SeedGovernorates()
                .SeedCities();

            base.OnModelCreating(builder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseLazyLoadingProxies()
                .UseSqlServer(@"Data Source=.; Initial Catalog=Mafqood; 
                    Integrated Security=True; TrustServerCertificate=True");
            base.OnConfiguring(optionsBuilder);
        }

    }
}