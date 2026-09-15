using System.Security.Claims;
using FluentResults;
using ResumeSystemManagement.Core.Entities;

namespace ResumeSystemManagement.Core.Interfaces;

public interface IUserRepository
{
    Task<User?> GetUserById(string id);
    Task<User?> GetUserByEmail(string email);
    Task<User?> GetUserByEmail(string email, string password);
    Task<bool> CheckPassword(string userId, string password);
    Task<Result<User>> CreateCandidate(string userName, string email, string password);
    Task<Result<User>> CreateExternalUser(string email, ClaimsPrincipal userPrincipal, string provider);
    Task<Result<User>> CreateUserLogin(string provider, ClaimsPrincipal claimsPrincipal, string email);
    Task<Result<User>> AssignRole(string userId, string role);
    Task<bool> HasExternalLogin(string provider, string providerKey);
    Task<string?> GetRoles(string userId);
}