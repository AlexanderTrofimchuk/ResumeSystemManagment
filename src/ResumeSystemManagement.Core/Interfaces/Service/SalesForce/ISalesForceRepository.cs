using FluentResults;

namespace ResumeSystemManagement.Core.Interfaces.Service.SalesForce;

public interface ISalesForceRepository
{
    Task<Result<string>> CreateSalesForceAccount(string jsonAccount);
    Task<Result<string>> CreateSalesForceContact(string jsonContact);
}