using System.Security.Claims;
using FluentResults;
using Microsoft.AspNetCore.Identity;
using ResumeSystemManagement.Core.Entities;
using ResumeSystemManagement.Core.Interfaces.Repositories.UserRepository;
using ResumeSystemManagement.Core.ReadModels;
using ResumeSystemManagement.Infrastructure.Context;
using ResumeSystemManagement.Infrastructure.IdentityEntities;
using ResumeSystemManagement.Infrastructure.Mappers;

namespace ResumeSystemManagement.Infrastructure.Repositories;

public class UserRepository(UserManager<AppUser> userManager, ApplicationDbContext context) : IUserRepository
{
    private readonly UserManager<AppUser> _userManager = userManager;
    private readonly ApplicationDbContext _context = context;
    
    public async Task<User?> GetUserById(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return null;
        var roles = await _userManager.GetRolesAsync(user);
        return user.ToUser(roles.ToList()[0]);
    }
    
    private async Task<AppUser?> GetAppUserById(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return null;
        return user;
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) return null;
        var role = await GetRoles(user.Id);
        return user.ToUser(role);
    }
    
    public async Task<User?> GetUserByEmail(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user?.UserName == null || user.Email == null) return null;
        var roles = await _userManager.GetRolesAsync(user);
        return new User{Id = user.Id, UserName = user.UserName,Email = user.Email, Role = roles.ToList()[0]};
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

    public async Task<Result<User>> CreateCandidate(string userName, string email, string password)
    {
        return await CreateUserWithRole(() => CreateUser(userName, email, password), RoleNames.Candidate);
    }

    public async Task<Result<User>> CreateExternalUser(string email, ClaimsPrincipal userPrincipal, string provider)
    {
        return await CreateUserWithRole(() => CreateExternalUserResult(email, userPrincipal,provider), RoleNames.Candidate);
    }

    public async Task<Result<User>> CreateUserLogin(string provider,ClaimsPrincipal claimsPrincipal, string email)
    {
        var loginInfo = CreateLoginInfo(provider, 
            claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty,provider);
        var user = await GetAppUserByEmail(email);
        if  (user == null) return Result.Fail("user not found");
        var loginsResult =  await _userManager.AddLoginAsync(user, loginInfo);
        return loginsResult.Succeeded ? Result.Ok(user.ToUser()) : Result.Fail(loginsResult.Errors.Select(e => e.Description));
    }

    private async Task<Result<User>> CreateUser(string userName, string email, string password)
    {
        var newUser = await CreateAppUser(email, userName, password);
        var result = await _userManager.CreateAsync(newUser, password);
        return result.Succeeded ? Result.Ok(newUser.ToUser()) : Result.Fail(result.Errors.Select(e => e.Description));
    }

    private async Task<Result<User>> CreateExternalUserResult(string email, ClaimsPrincipal userPrincipal, string provider)
    {
        var newUser = await CreateAppUser(email, GetFullName(userPrincipal),isExternalLogin: true);
        var result = await _userManager.CreateAsync(newUser);
        if (!result.Succeeded) return Result.Fail(result.Errors.Select(e => e.Description));
        var loginInfo = CreateLoginInfo(provider, userPrincipal.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty, provider);
        var loginResult = await _userManager.AddLoginAsync(newUser,loginInfo);
        if  (!loginResult.Succeeded) return Result.Fail(loginResult.Errors.Select(e => e.Description));
        return Result.Ok(newUser.ToUser());
    }
    
    private async Task<Result<User>> CreateUserWithRole(
        Func<Task<Result<User>>> createUser, string role)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
    
        var userResult = await createUser();
        if (userResult.IsFailed) return userResult;
    
        var assignResult = await AssignRole(userResult.Value.Id, role);
        if (assignResult.IsFailed) return assignResult;
    
        await transaction.CommitAsync();
        return Result.Ok(userResult.Value);
    }

    public async Task<Result<User>> AssignRole(string userId, string role)
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
}