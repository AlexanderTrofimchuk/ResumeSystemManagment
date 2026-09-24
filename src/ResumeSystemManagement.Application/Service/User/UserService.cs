using FluentResults;
using ResumeSystemManagement.Application.DTOs.User;
using ResumeSystemManagement.Application.Interfaces.User;
using ResumeSystemManagement.Core.Interfaces.Repositories.UserRepository;
using ResumeSystemManagement.Core.ReadModels;

namespace ResumeSystemManagement.Application.Service.User;

public class UserService(IUserRepository repository) : IUserService
{
    private readonly IUserRepository _userRepository = repository;
    
    public async Task<UserDetails> GetAllRecordAsync(int pageNumber, int pageSize)
    {
        return new() 
        {
            UserInfos = await _userRepository.GetAllAsync(pageNumber),
            CurrentPage = pageNumber,
            PageSize = PaginationConstants.DefaultPageSize,
            TotalCount = await _userRepository.GetUsersCount()
        };
    }

    public async Task<Result> ChangeRoleAsync(List<string> selectedIds, string role)
    {
        var result = await HasOneAdministrator(selectedIds);
        if (result.IsFailed) return result;
        return await Result.Try(() => _userRepository.ChangeUsersRole(selectedIds, role));
    }

    public Task<Result> ChangePasswordAsync(string email, string oldPassword, string newPassword)
    {
        return _userRepository.ChangePassword(email, oldPassword, newPassword);
    }

    public async Task<Result> BlockUserAsync(List<string> userIds)
    { 
        var result = await HasOneAdministrator(userIds);
        if (result.IsFailed) return result;
        return await Result.Try(() => _userRepository.BlockedUsers(userIds));
    }

    public async Task<Result> UnblockUserAsync(List<string> userIds)
    {
        var result = await HasOneAdministrator(userIds);
        if (result.IsFailed) return result;
        return await Result.Try(() => _userRepository.UnBlockedUsers(userIds));
    }

    public async Task<Result> DeleteUsersAsync(List<string> userIds)
    {
        var result = await HasOneAdministrator(userIds);
        if (result.IsFailed) return result;
        return await Result.Try(() => _userRepository.DeleteUsers(userIds));
    }
    
    private async Task<Result> HasOneAdministrator(List<string> userIds)
    {
        var adminIds = await _userRepository.GetAdminIdes();
        var remainingAdmins = adminIds.Except(userIds).ToList();
        if (remainingAdmins.Count == 0) 
            return Result.Fail("The system cannot be left without an administrator.");
        return Result.Ok();
    }
}