using FluentResults;

namespace ResumeSystemManagement.Core.Interfaces.Service.Registration;

public interface IRegistrationService
{
    Task<Result> Register(string userName, string email, string password);
}