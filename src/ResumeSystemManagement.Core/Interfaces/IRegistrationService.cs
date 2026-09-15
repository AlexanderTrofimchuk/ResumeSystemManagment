using FluentResults;

namespace ResumeSystemManagement.Core.Interfaces;

public interface IRegistrationService
{
    Task<Result> Register(string userName, string email, string password);
}