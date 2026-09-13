namespace ResumeSystemManagement.Application.Interfaces;

public interface IAuthService
{
    Task Login(string id, string email, string role);
    Task Logout();
}