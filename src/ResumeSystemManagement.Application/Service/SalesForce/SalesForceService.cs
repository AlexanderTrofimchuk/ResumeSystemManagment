using System.Text.Json;
using FluentResults;
using ResumeSystemManagement.Application.DTOs.SalesForce;
using ResumeSystemManagement.Application.Interfaces.Auth;
using ResumeSystemManagement.Application.Interfaces.SalesForce;
using ResumeSystemManagement.Application.Mappers;
using ResumeSystemManagement.Core.Interfaces.Repositories.AttributeRepository;
using ResumeSystemManagement.Core.Interfaces.Repositories.UserRepository;
using ResumeSystemManagement.Core.Interfaces.Service.SalesForce;

namespace ResumeSystemManagement.Application.Service.SalesForce;

public class SalesForceService(ISalesForceRepository salesForceRepository, ICandidateAttributeRepository candidateAttributeRepository, IUserRepository userRepository, IUserContext userContext) : ISalesForceService
{
    private readonly ISalesForceRepository _salesForceRepository = salesForceRepository;
    private readonly ICandidateAttributeRepository _candidateAttributeRepository = candidateAttributeRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IUserContext _userContext = userContext;

    private static string SerializeJson(object account)
    {
        var json = JsonSerializer.Serialize(account);
        return json;
    }

    public async Task<CreateAccountSalesForce> GetInfoForForm(string userId)
    {
        var buildInAttributes = await _candidateAttributeRepository.GetBuildInAttribute(userId);
        return new CreateAccountSalesForce()
        {
            Account = new(),
            Contact = buildInAttributes.ToContactInfo()
        };
    }

    public async Task<Result> CreateAccountAndContact(CreateAccountSalesForce accountInfo)
    {
        var jsonAccount = SerializeJson(accountInfo.Account);
        var accountId = await _salesForceRepository.CreateSalesForceAccount(jsonAccount);
        if (accountId.IsFailed) return accountId.ToResult();
        accountInfo.Contact.AccountId = accountId.Value;
        var jsonContact = SerializeJson(accountInfo.Contact);
        var contactId = await _salesForceRepository.CreateSalesForceContact(jsonContact);
        if (contactId.IsFailed) return contactId.ToResult();
        
        return await CreateSalesForceAccount(_userContext.UserId.ToString(), contactId.Value, accountId.Value);
    }

    public Task<bool> HasAccountAndContact()
    {
        return _userRepository.HasSalesForceAccount(_userContext.UserId.ToString());
    }

    private Task <Result> CreateSalesForceAccount(string userId, string contactId, string accountId)
    {
        return _userRepository.SetSalesForceAccount(userId, accountId, contactId);
    }
}