using FluentResults;

namespace ResumeSystemManagement.Core.Interfaces.Service.SalesForce;

public interface ISalesForceRepository
{
    Task<Result<string>> CreateAccount(string jsonAccount);
    Task<Result<string>> CreateContact(string jsonContact);
    Task<Result> DeleteAccount(string accountId);
}