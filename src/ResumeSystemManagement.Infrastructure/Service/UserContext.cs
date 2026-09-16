using Microsoft.AspNetCore.Http;
using ResumeSystemManagement.Application.Interfaces.Auth;
using ResumeSystemManagement.Infrastructure.Extensions;

namespace ResumeSystemManagement.Infrastructure.Service;

public class UserContext(IHttpContextAccessor accessor): IUserContext
{
    public bool IsAuthenticated => 
        accessor
            .HttpContext?
            .User
            .Identity?
            .IsAuthenticated ?? 
        throw new ArgumentException("User context is unavailable");
    
    public Guid UserId => 
        accessor
            .HttpContext?
            .User
            .GetUserId() ??
        throw new ArgumentException("User context is unavailable");
    
    public string Role =>   
        accessor
        .HttpContext?
        .User
        .GetRole() ??
        throw new ArgumentException("User context is unavailable");
}