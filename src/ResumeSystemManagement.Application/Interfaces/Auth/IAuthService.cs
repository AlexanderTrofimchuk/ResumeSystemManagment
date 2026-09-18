namespace ResumeSystemManagement.Application.Interfaces.Auth;

public interface IAuthService
{
    Task Login(string id, string email, string role);
    Task Logout();
}