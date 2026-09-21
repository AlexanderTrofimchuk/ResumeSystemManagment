using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ResumeSystemManagement.Application.Interfaces.Auth;
using ResumeSystemManagement.Core.Interfaces.Repositories.AttributeRepository;
using ResumeSystemManagement.Core.Interfaces.Repositories.PositionRepository;
using ResumeSystemManagement.Core.Interfaces.Repositories.UserRepository;
using ResumeSystemManagement.Infrastructure.Context;
using ResumeSystemManagement.Infrastructure.IdentityEntities;
using ResumeSystemManagement.Infrastructure.Interfaces;
using ResumeSystemManagement.Infrastructure.Repositories.Attributes;
using ResumeSystemManagement.Infrastructure.Repositories.Positions;
using ResumeSystemManagement.Infrastructure.Repositories.User;
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
            options.LoginPath = "/Account/Login";
            options.AccessDeniedPath = "/Account/Login";
            options.Cookie.SameSite = SameSiteMode.None;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        });
        
        services.AddExternalAuthentication();
        services.AddHttpContextAccessor();
        services.AddScoped<IUserRepository,UserRepository>();
        services.AddScoped<IAttributeLibraryRepository, AttributeLibraryRepository>();
        services.AddScoped<IAttributeCategoryRepository, AttributeCategoryRepository>();
        services.AddScoped<IAttributeTypeRepository, AttributeTypeRepository>();
        services.AddScoped<IPositionRepository,PositionRepository>();
        services.AddScoped<IPositionTemplateRepository, PositionTemplateRepository>();
        services.AddScoped<IUserContext, UserContext>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IExternalAuthProvider, ExternalAuthProvider>();
        return services;
    }
    
    public static async Task InitializeDatabase(this IServiceProvider serviceProvider)
    {
        Console.WriteLine("Initializing database and roles...");
        await using var scope = serviceProvider.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await dbContext.Database.MigrateAsync();
        await scope.ServiceProvider.SeedIdentityAsync();
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