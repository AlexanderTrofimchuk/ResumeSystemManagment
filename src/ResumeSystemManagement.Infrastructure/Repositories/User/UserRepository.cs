using System.Security.Claims;
using FluentResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ResumeSystemManagement.Core.Entities;
using ResumeSystemManagement.Core.Interfaces.Repositories.UserRepository;
using ResumeSystemManagement.Core.ReadModels;
using ResumeSystemManagement.Infrastructure.Context;
using ResumeSystemManagement.Infrastructure.IdentityEntities;
using ResumeSystemManagement.Infrastructure.Mappers;

namespace ResumeSystemManagement.Infrastructure.Repositories.User;

public class UserRepository(UserManager<AppUser> userManager, ApplicationDbContext context) : IUserRepository
{
    private readonly UserManager<AppUser> _userManager = userManager;
    private readonly ApplicationDbContext _context = context;
    
    public async Task<Core.Entities.UserInfo?> GetUserById(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return null;
        var roles = await _userManager.GetRolesAsync(user);
        return user.ToUser(roles.ToList()[0]);
    }

    public Task<int> GetUsersCount()
    {
        return _userManager.Users.CountAsync();
    }

    private async Task<AppUser?> GetAppUserById(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return null;
        return user;
    }

    public async Task<Core.Entities.UserInfo?> GetUserByEmail(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) return null;
        var role = await GetRoles(user.Id);
        return user.ToUser(role);
    }

    public Task<bool> UserIsBlocked(string userId)
    {
        return _context.Users
            .Where(u => u.Id == userId)
            .Select(u => u.LockoutEnd > DateTimeOffset.UtcNow)
            .FirstOrDefaultAsync();
    }

    public Task<List<UserInfo>> GetAllAsync(int pageNumber, int pageSize = 50)
    {
        return _userManager.Users
            .AsNoTracking()
            .OrderBy(u => u.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Join(_context.UserRoles,
                u => u.Id,
                ur => ur.UserId,
                (u, ur) => new { u, ur.RoleId })
            .Join(_context.Roles,
                (ur) => ur.RoleId,
                ir => ir.Id,
                (ur, ir) => new { ur.u, ir.Name})
            .Select(r => r.u.ToUser(r.Name,r.u.LockoutEnd > DateTimeOffset.UtcNow))
            .ToListAsync();
    }

    private async Task<AppUser?> GetAppUserByEmail(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        return user ?? null;
    }
    
    public async Task<bool> CheckPassword(string userId, string password)
    {
        var user = await GetAppUserById(userId);
        if (user == null) return false;
        return await _userManager.CheckPasswordAsync(user, password);
    }

    public async Task<Result<Core.Entities.UserInfo>> CreateCandidate(string userName, string email, string password)
    {
        return await CreateUserWithRole(() => CreateUser(userName, email, password), RoleNames.Candidate);
    }

    public async Task<Result<Core.Entities.UserInfo>> CreateExternalUser(string email, ClaimsPrincipal userPrincipal, string provider)
    {
        return await CreateUserWithRole(() => CreateExternalUserResult(email, userPrincipal,provider), RoleNames.Candidate);
    }

    public async Task<Result<Core.Entities.UserInfo>> CreateUserLogin(string provider,ClaimsPrincipal claimsPrincipal, string email)
    {
        var loginInfo = CreateLoginInfo(provider, 
            claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty,provider);
        var user = await GetAppUserByEmail(email);
        if  (user == null) return Result.Fail("user not found");
        var loginsResult =  await _userManager.AddLoginAsync(user, loginInfo);
        return loginsResult.Succeeded ? Result.Ok(user.ToUser()) : Result.Fail(loginsResult.Errors.Select(e => e.Description));
    }

    private async Task<Result<Core.Entities.UserInfo>> CreateUser(string userName, string email, string password)
    {
        var newUser = await CreateAppUser(email, userName, password);
        var result = await _userManager.CreateAsync(newUser, password);
        return result.Succeeded ? Result.Ok(newUser.ToUser()) : Result.Fail(result.Errors.Select(e => e.Description));
    }

    private async Task<Result<Core.Entities.UserInfo>> CreateExternalUserResult(string email, ClaimsPrincipal userPrincipal, string provider)
    {
        var newUser = await CreateAppUser(email, GetFullName(userPrincipal),isExternalLogin: true);
        var result = await _userManager.CreateAsync(newUser);
        if (!result.Succeeded) return Result.Fail(result.Errors.Select(e => e.Description));
        var loginInfo = CreateLoginInfo(provider, userPrincipal.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty, provider);
        var loginResult = await _userManager.AddLoginAsync(newUser,loginInfo);
        if  (!loginResult.Succeeded) return Result.Fail(loginResult.Errors.Select(e => e.Description));
        return Result.Ok(newUser.ToUser());
    }
    
    private async Task<Result<Core.Entities.UserInfo>> CreateUserWithRole(
        Func<Task<Result<Core.Entities.UserInfo>>> createUser, string role)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
    
        var userResult = await createUser();
        if (userResult.IsFailed) return userResult;
    
        var assignResult = await AssignRole(userResult.Value.Id, role);
        if (assignResult.IsFailed) return assignResult;
    
        await transaction.CommitAsync();
        return Result.Ok(userResult.Value);
    }

    public async Task<Result<Core.Entities.UserInfo>> AssignRole(string userId, string role)
    {
        var user = await GetAppUserById(userId);
        if (user == null) return Result.Fail("user not found");
        var result = await _userManager.AddToRoleAsync(user, role);
        return result.Succeeded ? Result.Ok(user.ToUser()) : Result.Fail(result.Errors.Select(e => e.Description));
    }

    public async Task<bool> HasExternalLogin(string provider, string providerKey)
    {
        return await _userManager.FindByLoginAsync(provider, providerKey) is not null;
    }

    public async Task ChangeUsersRole(List<string> userIds, string role)
    {
        var users = await GetSelectedUsers(userIds);
        
        foreach (var user in users)
        {
            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);
            await _userManager.AddToRoleAsync(user, role);
            await _userManager.UpdateSecurityStampAsync(user);
        }
    }

    public async Task<Result> ChangePassword(string email, string oldPassword, string newPassword)
    {
        if (oldPassword == newPassword) return Result.Fail("New password must be different from the old password");
        var user = await GetAppUserByEmail(email);
        if (user is null) return Result.Fail("User not found");
        var resultChange = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
        return !resultChange.Succeeded ?  
            Result.Fail(resultChange.Errors.Select(e => e.Description)) : Result.Ok();
    }
    
    public async Task BlockedUsers(List<string> userIds)
    {
        var users = await GetSelectedUsers(userIds);
        foreach (var user in users)
            await _userManager.UpdateSecurityStampAsync(user);
        
        await _context.Users.Where(u => userIds.Contains(u.Id))
            .ExecuteUpdateAsync(
                s => s.SetProperty(p => p.LockoutEnd, u => DateTimeOffset.MaxValue));
    }

    public async Task UnBlockedUsers(List<string> userIds)
    {
        var users = await GetSelectedUsers(userIds);
        foreach (var user in users)
            await _userManager.UpdateSecurityStampAsync(user);
        
        await  _userManager.Users.Where(u => userIds.Contains(u.Id))
            .ExecuteUpdateAsync(
                s => s.SetProperty(p => p.LockoutEnd, u => null));
    }

    public async Task DeleteUsers(List<string> userIds)
    {
        var users = await GetSelectedUsers(userIds);
        foreach (var user in users)
            await _userManager.UpdateSecurityStampAsync(user);
        
        await _context.Users.Where(u => userIds.Contains(u.Id))
            .ExecuteDeleteAsync();
    }

    public async Task<string?> GetRoles(string userId)
    {
        var user = await GetAppUserById(userId);
        if (user == null) return null;
        var roles = await _userManager.GetRolesAsync(user);
        return roles.ToList()[0];
    }

    private static string GetFullName(ClaimsPrincipal userPrincipal)
    {
        var userName = $"{userPrincipal.FindFirstValue(ClaimTypes.GivenName) ?? string.Empty} " +
                       $"{userPrincipal.FindFirstValue(ClaimTypes.Surname) ?? string.Empty}";
        return userName;
    }

    private async Task<AppUser> CreateAppUser(string email, string fullName,string? password = null, bool isExternalLogin = false)
    {
        var newUser = new AppUser
        {
            UserName = email, 
            Email = email, 
            FullName = fullName,
        };
        if (password != null)
            await _userManager.AddPasswordAsync(newUser, password);
        
        if (isExternalLogin)
            newUser.EmailConfirmed =  true;
        
        return newUser;
    }

    private static UserLoginInfo CreateLoginInfo(string loginProvider, string providerKey,  string providerDisplayName)
    {
        return new UserLoginInfo(loginProvider, providerKey, providerDisplayName);
    }

    public async Task<HashSet<string>> GetAdminIdes()
    {
        var users = await _userManager.GetUsersInRoleAsync(RoleNames.Administrator);
        return users.Select(u => u.Id).ToHashSet();
    }
    
    private Task<List<AppUser>> GetSelectedUsers(List<string> userIds)
    {
        return _userManager.Users
            .Where(u => userIds.Contains(u.Id))
            .ToListAsync();
    }
}