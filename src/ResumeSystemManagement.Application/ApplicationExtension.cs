using Microsoft.Extensions.DependencyInjection;
using ResumeSystemManagement.Application.Interfaces;
using ResumeSystemManagement.Application.Service.Login;
using ResumeSystemManagement.Application.Service.Registration;
using ResumeSystemManagement.Core.Interfaces;

namespace ResumeSystemManagement.Application;

public static class ApplicationExtension
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ILoginService,CredentialLogin>();
        services.AddScoped<IExternalLoginService, ExternalLoginService>();
        services.AddScoped<IRegistrationService, RegistrationService>();
        
        return services;
    }
}