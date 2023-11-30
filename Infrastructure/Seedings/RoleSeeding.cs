using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Seedings
{
    public static class RoleSeeding
    {
        public static ModelBuilder SeedRoles(this ModelBuilder builder)
        {
            builder
                .Entity<IdentityRole<Guid>>()
                .HasData
                (
                new IdentityRole<Guid> { Id = Guid.Parse("5638ecb0-eadd-453e-83c4-c060a55cd384"), Name = "admin", NormalizedName = "ADMIN" }
                );
            return builder;
        }
    }
}
