using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ResumeSystemManagement.Application.Interfaces;
using ResumeSystemManagement.Core.Interfaces;
using ResumeSystemManagement.Core.StaticDatas;
using ResumeSystemManagement.Infrastructure.Context;
using ResumeSystemManagement.Infrastructure.IdentityEntities;
using ResumeSystemManagement.Infrastructure.Interfaces;
using ResumeSystemManagement.Infrastructure.Repositories;
using ResumeSystemManagement.Infrastructure.Service;

namespace ResumeSystemManagement.Infrastructure;

public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructureServices
        (this IServiceCollection services)
    {
        services.AddDbContext<ApplicationDbContext>();
        services.AddIdentity<AppUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();
        
        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequiredLength = 8;
            
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromHours(24);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;
            
            options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            options.User.RequireUniqueEmail = true;
        });
        
        services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.SameSite = SameSiteMode.None;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        });
        
        services.AddExternalAuthentication();
        services.AddHttpContextAccessor();
        services.AddScoped<IUserRepository,UserRepository>();
        services.AddScoped<IUserContext, UserContext>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IExternalAuthProvider, ExternalAuthProvider>();
        return services;
    }
    
    public static async Task InitializeDbAndRoles(this IServiceProvider serviceProvider)
    {
        Console.WriteLine("Initializing database and roles...");
        var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
        await dbContext.Database.MigrateAsync();
        await serviceProvider.SeedRoles();
    }

    private static async Task SeedRoles(this IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        foreach (string role in RoleNames.Roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    private static void AddExternalAuthentication(this IServiceCollection services)
    {
        services.AddAuthentication()
            .AddGoogle("Google", googleOptions =>
            {
                googleOptions.ClientId = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_ID")!;
                googleOptions.ClientSecret = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_SECRET")!;
                googleOptions.SignInScheme = IdentityConstants.ExternalScheme;
            })
            .AddFacebook("Facebook", facebookOptions =>
            {
                facebookOptions.AppId = Environment.GetEnvironmentVariable("FACEBOOK_APP_ID")!;
                facebookOptions.AppSecret = Environment.GetEnvironmentVariable("FACEBOOK_APP_SECRET")!;
                facebookOptions.Fields.Add("email");
                facebookOptions.SignInScheme =  IdentityConstants.ExternalScheme;
            });
    }
}