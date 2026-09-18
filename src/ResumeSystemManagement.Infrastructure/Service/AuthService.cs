using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using ResumeSystemManagement.Application.Interfaces.Auth;

namespace ResumeSystemManagement.Infrastructure.Service;

public class AuthService(IHttpContextAccessor accessor): IAuthService
{
    public async Task Login(string id, string email, string role)
    {
        var principal = CreateClaimsPrincipal(id, email, role);
        await accessor.HttpContext!.SignInAsync(IdentityConstants.ApplicationScheme, principal);
    }

    public async Task Logout()
    {
        await accessor.HttpContext!.SignOutAsync(IdentityConstants.ApplicationScheme);
        await accessor.HttpContext!.SignOutAsync(IdentityConstants.ExternalScheme);
    }

    private static ClaimsPrincipal CreateClaimsPrincipal(string id, string email, string role)
    {
        var claims =new List<Claim> 
            { new Claim(ClaimTypes.NameIdentifier, id), 
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, role)
            };
        var identity = new ClaimsIdentity(claims, IdentityConstants.ApplicationScheme);
        return new ClaimsPrincipal(identity);
    }
}