using FluentResults;
using ResumeSystemManagement.Core.Entities;
using ResumeSystemManagement.Core.Interfaces;

namespace ResumeSystemManagement.Application.Interfaces;

public abstract class LoginServiceBase(IAuthService authService, IUserRepository userRepository)
{
    protected readonly IUserRepository UserRepository = userRepository;
    protected readonly IAuthService AuthService = authService;
    
    protected async Task<Result> AuthorizeUser(User user, string role)
    { 
        await AuthService.Login(user.Id, user.Email, role);
        return Result.Ok();
    }
    
    protected async Task<Result<(User user, string role)>> GetUserRole(User user)
    {
        var role = await UserRepository.GetRoles(user.Id);
        if (role is null) return Result.Fail("Role not found");
        return Result.Ok((user, role));
    }
}