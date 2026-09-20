using Microsoft.Extensions.DependencyInjection;
using ResumeSystemManagement.Application.Interfaces.Attributes;
using ResumeSystemManagement.Application.Interfaces.Logins;
using ResumeSystemManagement.Application.Interfaces.Position;
using ResumeSystemManagement.Application.Service.Attributes;
using ResumeSystemManagement.Application.Service.Login;
using ResumeSystemManagement.Application.Service.Position;
using ResumeSystemManagement.Application.Service.Registration;
using ResumeSystemManagement.Core.Interfaces.Service.Login;
using ResumeSystemManagement.Core.Interfaces.Service.Registration;

namespace ResumeSystemManagement.Application;

public static class ApplicationExtension
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ILoginService,CredentialLogin>();
        services.AddScoped<IExternalLoginService, ExternalLoginService>();
        services.AddScoped<IRegistrationService, RegistrationService>();
        services.AddScoped<IAttributeLibraryService, AttributeLibraryService>();
        services.AddScoped<IAttributeTypeService, AttributeTypeService>();
        services.AddScoped<IAttributeCategoryService, AttributeCategoryService>();
        services.AddScoped<IPositionService, PositionService>();
        services.AddScoped<IPositionTemplateService, PositionTemplateService>();
        
        return services;
    }
}