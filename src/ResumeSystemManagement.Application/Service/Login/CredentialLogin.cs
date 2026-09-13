using FluentResults;
using FluentResults.Extensions;
using ResumeSystemManagement.Application.Interfaces;
using ResumeSystemManagement.Core.Entities;
using ResumeSystemManagement.Core.Interfaces;

namespace ResumeSystemManagement.Application.Service.Login;

public class CredentialLogin(IAuthService authService, IUserRepository userRepository) : LoginServiceBase(authService, userRepository),ILoginService
{

    public async Task<Result> Login(string email, string password)
    {
        return await ValidateCredentials(email, password)
            .Bind(GetUserRole)
            .Bind(data => AuthorizeUser(data.user, data.role));
    }

    public async Task Logout()
    {
        await authService.Logout();
    }

    private async Task<Result<User>> ValidateCredentials(string email, string password)
    {
        var user = await userRepository.GetUserByEmail(email, password);
        if (user is null) return Result.Fail("User not Found");
        if (!await userRepository.CheckPassword(user.Id, password))
            return Result.Fail("Password Incorrect");
        return Result.Ok(user);
    }
}