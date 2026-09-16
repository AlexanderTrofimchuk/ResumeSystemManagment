namespace ResumeSystemManagement.Application.Interfaces.Auth;

public interface IUserContext
{
    bool IsAuthenticated { get; }
    Guid UserId { get; }
    string Role { get; }
}