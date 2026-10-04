using Microsoft.Extensions.DependencyInjection;
using ResumeSystemManagement.Application.Interfaces.Attributes;
using ResumeSystemManagement.Application.Interfaces.Login;
using ResumeSystemManagement.Application.Interfaces.Positions;
using ResumeSystemManagement.Application.Interfaces.Statistics;
using ResumeSystemManagement.Application.Interfaces.Ticket;
using ResumeSystemManagement.Application.Interfaces.Users;
using ResumeSystemManagement.Application.Service.Attributes;
using ResumeSystemManagement.Application.Service.Login;
using ResumeSystemManagement.Application.Service.Positions;
using ResumeSystemManagement.Application.Service.Registration;
using ResumeSystemManagement.Application.Service.Statistics;
using ResumeSystemManagement.Application.Service.Ticket;
using ResumeSystemManagement.Application.Service.Users;
using ResumeSystemManagement.Core.Interfaces.Service.Login;
using ResumeSystemManagement.Core.Interfaces.Service.Registration;

namespace ResumeSystemManagement.Application;

public static class ApplicationExtension
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ILoginService,CredentialLogin>();
        services.AddScoped<IExternalLoginService, ExternalLoginService>();
        services.AddScoped<IRegistrationService, RegistrationService>();
        services.AddScoped<IAttributeLibraryService, AttributeLibraryService>();
        services.AddScoped<IAttributeTypeService, AttributeTypeService>();
        services.AddScoped<IAttributeCategoryService, AttributeCategoryService>();
        services.AddScoped<IPositionService, PositionService>();
        services.AddScoped<IPositionTemplateService, PositionTemplateService>();
        services.AddScoped<IStatisticsService,StatisticsService>();
        services.AddScoped<IProfileService, ProfileService>();
        services.AddScoped<ISupportTicketService, SupportTicketService>();
        return services;
    }
}