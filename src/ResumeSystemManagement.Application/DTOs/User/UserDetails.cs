using ResumeSystemManagement.Core.Entities;

namespace ResumeSystemManagement.Application.DTOs.User;

public class UserDetails
{
    public List<UserInfo> UserInfos { get; set; } = new();
    public List<string> SelectIds { get; set; } = new();
    public int CurrentPage { get; set; } = 1;
    public int PageSize { get; set; } = 50;
    public int TotalCount { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
}