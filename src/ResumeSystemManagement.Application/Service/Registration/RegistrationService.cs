using FluentResults;
using ResumeSystemManagement.Application.Interfaces.Auth;
using ResumeSystemManagement.Core.Interfaces.Repositories.UserRepository;
using ResumeSystemManagement.Core.Interfaces.Service.Registration;

namespace ResumeSystemManagement.Application.Service.Registration;

public class RegistrationService(IUserRepository repository, IAuthService authService): IRegistrationService
{
    public async Task<Result> Register(string userName, string email, string password)
    {
        var result = await repository.CreateCandidate(userName, email, password);
        if(!result.IsSuccess) return result.ToResult();
        var role = await repository.GetRoles(result.Value.Id);
        if (role is null) return Result.Fail("Not found roles");
        await authService.Login(result.Value.Id, result.Value.Email, role);
        return Result.Ok();
    }
}