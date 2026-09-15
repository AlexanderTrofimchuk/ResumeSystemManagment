namespace ResumeSystemManagement.Application.Interfaces;

public interface IUserContext
{
    bool IsAuthenticated { get; }
    Guid UserId { get; }
    string Role { get; }
}