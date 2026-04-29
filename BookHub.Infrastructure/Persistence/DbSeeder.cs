using BookHub.Domain.Entities;
using BookHub.Domain.Enums;

namespace BookHub.Infrastructure.Persistence;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        if (context.Companies.Any()) return;

        var company = new Company
        {
            Id = Guid.NewGuid(),
            Name = "Demo Barbershop",
            Slug = "demo-barbershop",
            CreatedAt = DateTime.UtcNow
        };

        var admin = new User
        {
            Id = Guid.NewGuid(),
            CompanyId = company.Id,
            FirstName = "Admin",
            LastName = "User",
            Email = "[EMAIL_ADDRESS]",
            PasswordHash = "hashed_password",
            Role = UserRole.Admin,
            CreatedAt = DateTime.UtcNow
        };

        context.Companies.Add(company);
        context.Users.Add(admin);
        await context.SaveChangesAsync();
    }
}