using System.Security.Claims;

namespace ResumeSystemManagement.Infrastructure.Extensions;

public static class ClaimsExtension
{
    public static Guid GetUserId(this ClaimsPrincipal? principal)
    {
        string? userId = principal?.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(userId, out Guid parsedUserId) ?
            parsedUserId :
            throw new ArgumentException("User id is unavailable");
    }

    public static string GetRole(this ClaimsPrincipal? principal)
    {
        var claims = principal?.FindAll(ClaimTypes.NameIdentifier);
        var roles = claims!.Where(c => c.Type == ClaimTypes.Role).ToList();
        return roles.Any() ? roles[0].Value : string.Empty;
    }
}