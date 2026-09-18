using FluentResults;
using FluentResults.Extensions;
using ResumeSystemManagement.Application.Interfaces.Auth;
using ResumeSystemManagement.Application.Interfaces.Logins;
using ResumeSystemManagement.Core.Entities;
using ResumeSystemManagement.Core.Interfaces.Repositories.UserRepository;
using ResumeSystemManagement.Core.Interfaces.Service.Login;

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
        await AuthService.Logout();
    }

    private async Task<Result<User>> ValidateCredentials(string email, string password)
    {
        var user = await UserRepository.GetUserByEmail(email, password);
        if (user is null) return Result.Fail("User not Found");
        if (!await UserRepository.CheckPassword(user.Id, password))
            return Result.Fail("Password Incorrect");
        return Result.Ok(user);
    }
}