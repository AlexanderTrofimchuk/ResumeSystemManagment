using FluentResults;
using ResumeSystemManagement.Application.DTOs.User;
using ResumeSystemManagement.Core.Entities;

namespace ResumeSystemManagement.Application.Interfaces.User;

public interface IUserService
{
    Task<UserDetails> GetAllRecordAsync(int pageNumber, int pageSize);
    Task<Result> ChangeRoleAsync(List<string> selectedIds, string role);
    Task<Result> ChangePasswordAsync(string email, string oldPassword, string newPassword);
    Task<Result> BlockUserAsync(List<string> userIds);
    Task<Result> UnblockUserAsync(List<string> userIds);
    Task<Result> DeleteUsersAsync(List<string> userIds);
}