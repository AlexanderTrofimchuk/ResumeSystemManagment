using System.Text.Json;
using FluentResults;
using ResumeSystemManagement.Application.DTOs.Ticket;
using ResumeSystemManagement.Application.Interfaces.Auth;
using ResumeSystemManagement.Application.Interfaces.FileStorage;
using ResumeSystemManagement.Application.Interfaces.Ticket;
using ResumeSystemManagement.Application.Mappers;
using ResumeSystemManagement.Core.Interfaces.Repositories.PositionRepository;
using ResumeSystemManagement.Core.Interfaces.Repositories.UserRepository;

namespace ResumeSystemManagement.Application.Service.Ticket;

public class SupportTicketService(
    IUserContext userContext,
    IPositionRepository positionRepository,
    IUserRepository userRepository,
    IFileStorage storage) : ISupportTicketService
{
    private readonly IPositionRepository _positionRepository = positionRepository;
    private readonly IUserContext _userContext = userContext;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IFileStorage _storage = storage;

    public async Task<Result> CreateTicketAsync(SupportTicket ticket)
    {
        var positionTitle = await GetPositionName(ticket);
        var admins = await GetAdmins();
        var userNameResult = await GetUserName(_userContext.UserId.ToString());
        if (userNameResult.IsFailed) return userNameResult.ToResult();
        var jsonEntity = ticket.MapToJson(userNameResult.Value,
            positionTitle, admins);
        var json = JsonSerializer.Serialize(jsonEntity);
        return await Result.Try(() =>
            _storage.SaveFileAsync($"ticket_{DateTime.UtcNow:yyyyMMdd_HHmmss}.json", json));
    }

    private async Task<string?> GetPositionName(SupportTicket ticket)
    { 
        if (ticket.PositionId == null) return string.Empty;
        return await _positionRepository.GetNAmeByIdAsync((int)ticket.PositionId);
    }

    private async Task<HashSet<string>> GetAdmins()
    {
        return await _userRepository.GetAdminEmails();
    }

    private async Task<Result<string>> GetUserName(string userId)
    {
        var username = await _userRepository.GetNameWithRole(userId);
        if (username == string.Empty)
            return Result.Fail("User not found");
        return Result.Ok(username);
    }
}