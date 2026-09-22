using ResumeSystemManagement.Core.Entities;
using ResumeSystemManagement.Infrastructure.IdentityEntities;

namespace ResumeSystemManagement.Infrastructure.Mappers;

public static class UserMapper
{
    public static UserInfo ToUser(this AppUser user, string? role = null, bool isBlocked = false)
    {
        return new UserInfo
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email!,
            Role = role,
            IsBlocked = isBlocked,
            SecurityStamp = user.SecurityStamp
        };
    }
}