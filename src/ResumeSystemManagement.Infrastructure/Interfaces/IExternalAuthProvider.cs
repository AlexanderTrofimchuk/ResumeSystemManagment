using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace ResumeSystemManagement.Infrastructure.Interfaces;

public interface IExternalAuthProvider
{
    Task<ClaimsPrincipal?> GetFacebookPrincipal();
    AuthenticationProperties ConfigureFacebookLogin(string? callbackUrl);
    Task<ClaimsPrincipal?> GetGooglePrincipal();
    AuthenticationProperties ConfigureGoogleLogin(string? callbackUrl);
}