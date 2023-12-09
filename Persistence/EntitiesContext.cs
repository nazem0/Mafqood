using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Persistence.DataSeeding;
using Persistence.EntitiesConfigs;

namespace Persistence
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
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder
        //        .UseLazyLoadingProxies()
        //        .UseSqlServer(@"Data Source=.; Initial Catalog=Mafqood; 
        //            Integrated Security=True; TrustServerCertificate=True");

        //    //optionsBuilder
        //    //    .UseLazyLoadingProxies()
        //    //    .UseSqlServer(@"Data Source=localhost; Initial Catalog=Mafqood;
        //    //        TrustServerCertificate=True; User Id=SA; Password=$aMer2030");

        //    base.OnConfiguring(optionsBuilder);
        //}

    }
}