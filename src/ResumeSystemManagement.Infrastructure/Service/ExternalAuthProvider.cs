using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using ResumeSystemManagement.Infrastructure.IdentityEntities;
using ResumeSystemManagement.Infrastructure.Interfaces;

namespace ResumeSystemManagement.Infrastructure.Service;

public class ExternalAuthProvider(SignInManager<AppUser> signManager, IHttpContextAccessor accessor) : IExternalAuthProvider
{
    public async Task<ClaimsPrincipal?> GetFacebookPrincipal()
    {
        var result = await accessor.HttpContext!.AuthenticateAsync(FacebookDefaults.AuthenticationScheme);
        return result.Succeeded ? result.Principal : null;
    }
    
    public async Task<ClaimsPrincipal?> GetGooglePrincipal()
    {
        var result = await accessor.HttpContext!.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
        return result.Succeeded ? result.Principal : null;
    }
    
    public AuthenticationProperties ConfigureGoogleLogin(string callbackUrl)
    {
       return signManager.ConfigureExternalAuthenticationProperties("Google", callbackUrl);
    }

    public AuthenticationProperties ConfigureFacebookLogin(string callbackUrl)
    {
        return signManager.ConfigureExternalAuthenticationProperties("Facebook", callbackUrl);
    }
}