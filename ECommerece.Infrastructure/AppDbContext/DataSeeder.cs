using ECommerece.Domain.Entities;
using ECommerece.Domain.Enums;
using ECommerece.Infrastructure.AppDbContext;
using Microsoft.AspNetCore.Identity;

namespace ECommerece.Infrastructure.Seed
{
    public class DataSeeder
    {
        public static async Task SeedAdminAsync(ECommerceDbContext context)
        {
            // ✅ امسح الـ Admin القديم لو موجود
            var existingAdmin = context.Users.FirstOrDefault(u => u.Role == UserRoles.Admin);
            if (existingAdmin != null)
                context.Users.Remove(existingAdmin);

            var passwordHasher = new PasswordHasher<User>();
            var admin = new User
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Admin",
                Email = "admin@admin.com",
                Role = UserRoles.Admin,
                CreatedAt = DateTime.Now,
                IsDeleted = false
            };

            admin.Password = passwordHasher.HashPassword(admin, "admin123");
            await context.Users.AddAsync(admin);
            await context.SaveChangesAsync();
        }
    }
}