using System.Security.Claims;
using FluentResults;
using ResumeSystemManagement.Core.Entities;

namespace ResumeSystemManagement.Core.Interfaces.Repositories.UserRepository;

public interface IUserRepository
{
    Task<int> GetUsersCount();
    Task<HashSet<string>> GetAdminIdes();
    Task<string?> GetRoles(string userId);
    Task<UserInfo?> GetUserById(string id);
    Task<int> GetCountCandidate();
    Task<int> GetCountRecruter();
    Task<UserInfo?> GetUserByEmail(string email);
    Task<bool> UserIsBlocked(string userId);
    Task<List<UserInfo>>  GetAllAsync(int pageNumber);
    Task<bool> CheckPassword(string userId, string password);
    Task<Result<UserInfo>> CreateCandidate(string userName, string email, string password);
    Task<Result<UserInfo>> CreateExternalUser(string email, ClaimsPrincipal userPrincipal, string provider);
    Task<Result<UserInfo>> CreateUserLogin(string provider, ClaimsPrincipal claimsPrincipal, string email);
    Task<Result<UserInfo>> AssignRole(string userId, string role);
    Task<bool> HasExternalLogin(string provider, string providerKey);
    Task ChangeUsersRole(List<string> userIds, string newRole);
    Task<Result> ChangePassword(string email, string oldPassword, string newPassword);
    Task BlockedUsers(List<string> userIds);
    Task UnBlockedUsers(List<string> userIds);
    Task DeleteUsers(List<string> userIds);
}