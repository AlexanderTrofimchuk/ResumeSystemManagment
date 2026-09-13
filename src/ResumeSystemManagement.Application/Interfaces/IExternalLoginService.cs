using System.Security.Claims;
using FluentResults;

namespace ResumeSystemManagement.Application.Interfaces;

public interface IExternalLoginService
{
    Task<Result> Login(ClaimsPrincipal claimsPrincipal,string provider);
}