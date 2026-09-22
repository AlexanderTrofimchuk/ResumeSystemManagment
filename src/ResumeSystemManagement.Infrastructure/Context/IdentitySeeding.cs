using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using ResumeSystemManagement.Core.ReadModels;

namespace ResumeSystemManagement.Infrastructure.Context;

public static class IdentitySeeding
{
    public static async Task SeedIdentityAsync(this IServiceProvider serviceProvider)
    {
        await serviceProvider.SeedRolesAsync();
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
}