namespace ResumeSystemManagement.Core.Entities;

public class UserInfo
{
    public string Id { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Role  { get; set; }
    public bool IsBlocked { get; set; }
    public string? SecurityStamp { get; set; }
}