using FluentResults;

namespace ResumeSystemManagement.Core.Interfaces;

public interface ILoginService
{
    Task<Result> Login(string email, string password);
    Task Logout();
}