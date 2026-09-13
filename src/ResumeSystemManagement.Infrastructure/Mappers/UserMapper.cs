using ResumeSystemManagement.Core.Entities;
using ResumeSystemManagement.Infrastructure.IdentityEntities;

namespace ResumeSystemManagement.Infrastructure.Mappers;

public static class UserMapper
{
    public static User ToUser(this AppUser user, string? role = null)
    {
        return new User
        {
            Id = user.Id,
            UserName = user.UserName!,
            Email = user.Email!,
            Role = role,
        };
    }
}