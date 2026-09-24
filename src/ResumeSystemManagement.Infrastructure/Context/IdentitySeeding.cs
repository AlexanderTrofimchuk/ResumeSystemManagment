using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using ResumeSystemManagement.Core.ReadModels;
using ResumeSystemManagement.Infrastructure.IdentityEntities;

namespace ResumeSystemManagement.Infrastructure.Context;

public static class IdentitySeeding
{
    public static async Task SeedIdentityAsync(this IServiceProvider serviceProvider)
    {
        await serviceProvider.SeedRolesAsync();
        await serviceProvider.CreateAdminUserAsync();
    }

    private static async Task SeedRolesAsync(this IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        foreach (string role in RoleNames.Roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    private static async Task CreateAdminUserAsync(this IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
        var email = Environment.GetEnvironmentVariable("ADMIN_EMAIL");
        var adminUser = new AppUser() { UserName = email, Email = email,
            FullName = "Admin Administrator", };
        var password = Environment.GetEnvironmentVariable("ADMIN_PASSWORD");
        await userManager.CreateAsync(adminUser, password!);
        await userManager.AddToRoleAsync(adminUser, RoleNames.Administrator);
    }
}