using FluentResults;
using ResumeSystemManagement.Application.Interfaces.Auth;
using ResumeSystemManagement.Core.Entities;
using ResumeSystemManagement.Core.Interfaces.Repositories.UserRepository;

namespace ResumeSystemManagement.Application.Interfaces.Logins;

public abstract class LoginServiceBase(IAuthService authService, IUserRepository userRepository)
{
    protected readonly IUserRepository UserRepository = userRepository;
    protected readonly IAuthService AuthService = authService;
    
    protected async Task<Result> AuthorizeUser(UserInfo userInfo, string role)
    { 
        await AuthService.Login(userInfo.Id, userInfo.Email, role, userInfo.SecurityStamp);
        return Result.Ok();
    }
    
    protected async Task<Result<(UserInfo user, string role)>> GetUserRole(UserInfo userInfo)
    {
        var role = await UserRepository.GetRoles(userInfo.Id);
        if (role is null) return Result.Fail("Role not found");
        return Result.Ok((user: userInfo, role));
    }
}