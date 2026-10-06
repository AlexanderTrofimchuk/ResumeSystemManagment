using FluentResults;
using ResumeSystemManagement.Application.DTOs.SalesForce;

namespace ResumeSystemManagement.Application.Interfaces.SalesForce;

public interface ISalesForceService
{
    Task<CreateAccountSalesForce> GetInfoForForm(string userId);
    Task<Result> CreateAccountAndContact(CreateAccountSalesForce accountInfo);
    Task<bool> HasAccountAndContact();
}