using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Persistence.DataSeeding
{
    public static class UserSeeding
    {
        public static PasswordHasher<User> PasswordHasher = new PasswordHasher<User>();
        public static ModelBuilder SeedUsers(this ModelBuilder builder)
        {
            builder
                .Entity<User>()
                .HasData(new User
                {
                    Id = Guid.Parse("da18f5d6-6034-436c-9393-1787b8b419fb"),
                    UserName = "admin",
                    NormalizedUserName = "ADMIN",
                    PasswordHash = PasswordHasher.HashPassword(null, "Asd@12345"),
                    SecurityStamp = Guid.NewGuid().ToString()
                });
            builder.Entity<IdentityUserRole<Guid>>().HasData(
            new IdentityUserRole<Guid>
            {
                RoleId = Guid.Parse("5638ecb0-eadd-453e-83c4-c060a55cd384"),
                UserId = Guid.Parse("da18f5d6-6034-436c-9393-1787b8b419fb")
            });
            return builder;
        }
    }
}
