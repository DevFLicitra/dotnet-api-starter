using Api.Domain;
using Microsoft.AspNetCore.Identity;

namespace Api.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext db)
    {
        if (db.Users.Any(u => u.Role == "Admin")) return;

        var hasher = new PasswordHasher<AppUser>();
        var admin = new AppUser
        {
            Email = "admin@admin.com",
            Role = "Admin"
        };
        admin.PasswordHash = hasher.HashPassword(admin, "Admin123!");

        db.Users.Add(admin);

        if (!db.Projects.Any())
        {
            db.Projects.AddRange(
                new Project { Name = "Demo Project 1", Description = "Seed data" },
                new Project { Name = "Demo Project 2", Description = "Seed data" }
            );
        }

        await db.SaveChangesAsync();
    }
}