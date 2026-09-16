using FluentResults;

namespace ResumeSystemManagement.Core.Interfaces.Service.Login;

public interface ILoginService
{
    Task<Result> Login(string email, string password);
    Task Logout();
}