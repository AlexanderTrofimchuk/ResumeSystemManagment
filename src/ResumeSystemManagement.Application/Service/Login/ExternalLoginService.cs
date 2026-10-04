using System.Security.Claims;
using FluentResults;
using FluentResults.Extensions;
using ResumeSystemManagement.Application.Interfaces.Auth;
using ResumeSystemManagement.Application.Interfaces.Login;
using ResumeSystemManagement.Core.Entities;
using ResumeSystemManagement.Core.Interfaces.Repositories.AttributeRepository;
using ResumeSystemManagement.Core.Interfaces.Repositories.UserRepository;

namespace ResumeSystemManagement.Application.Service.Login;

public class ExternalLoginService(IAuthService authService, IUserRepository userRepository, ICandidateAttributeRepository candidateAttributeRepository) :
    LoginServiceBase(authService, userRepository), IExternalLoginService
{
    public async Task<Result> Login(ClaimsPrincipal claimsPrincipal, string provider)
    {
        return await ValidateCredentials(claimsPrincipal,provider)
            .Bind(GetUserRole)
            .Bind(data => AuthorizeUser(data.user, data.role));
    }
    
    private async Task<Result<UserInfo>> ValidateCredentials(
        ClaimsPrincipal claimsPrincipal,
        string provider)
    {
        var email = claimsPrincipal.FindFirst(ClaimTypes.Email)?.Value;
        if (email is null) return Result.Fail("Email not found");

        var user = await UserRepository.GetUserByEmail(email);

        return user is null
            ? await CreateExternalUser(email, claimsPrincipal, provider)
            : await ValidateExistingUser(user, claimsPrincipal, provider);
    }

    private async Task<Result<UserInfo>> CreateExternalUser(
        string email,
        ClaimsPrincipal claimsPrincipal,
        string provider)
    {
        var result = await UserRepository.CreateExternalUser(email, claimsPrincipal, provider);
        if (result.IsFailed) return result;

        await candidateAttributeRepository.InitialBuildInAttributes(
            result.Value.Id,
            result.Value.FullName);

        return result;
    }

    private async Task<Result<UserInfo>> ValidateExistingUser(
        UserInfo user,
        ClaimsPrincipal claimsPrincipal,
        string provider)
    {
        if (await UserRepository.UserIsBlocked(user.Id))
            return Result.Fail("User is Blocked");

        var providerKey = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (providerKey is not null &&
            !await UserRepository.HasExternalLogin(provider, providerKey))
        {
            return await UserRepository.CreateUserLogin(provider, claimsPrincipal, user.Email);
        }

        return user;
    }
}