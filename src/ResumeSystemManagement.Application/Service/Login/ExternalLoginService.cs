using System.Security.Claims;
using FluentResults;
using FluentResults.Extensions;
using ResumeSystemManagement.Application.Interfaces.Auth;
using ResumeSystemManagement.Application.Interfaces.Logins;
using ResumeSystemManagement.Core.Entities;
using ResumeSystemManagement.Core.Interfaces.Repositories.UserRepository;

namespace ResumeSystemManagement.Application.Service.Login;

public class ExternalLoginService(IAuthService authService, IUserRepository userRepository) :
    LoginServiceBase(authService, userRepository), IExternalLoginService
{
    public async Task<Result> Login(ClaimsPrincipal claimsPrincipal, string provider)
    {
        return await ValidateCredentials(claimsPrincipal,provider)
            .Bind(GetUserRole)
            .Bind(data => AuthorizeUser(data.user, data.role));
    }

    private async Task<Result<UserInfo>> ValidateCredentials(ClaimsPrincipal claimsPrincipal, string provider)
    {
        var email = claimsPrincipal.FindFirst(ClaimTypes.Email)?.Value;
        if (email is null) return Result.Fail("Email not found");
        var user = await UserRepository.GetUserByEmail(email);
        if (user == null)
            return await UserRepository.CreateExternalUser(email, claimsPrincipal, provider);
        if (await UserRepository.UserIsBlocked(user.Id))  return Result.Fail("User is Blocked");
        var providerKey = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (providerKey != null && !await UserRepository.HasExternalLogin(provider, providerKey))
            return await UserRepository.CreateUserLogin(provider, claimsPrincipal, email);
        return user;
    }
}