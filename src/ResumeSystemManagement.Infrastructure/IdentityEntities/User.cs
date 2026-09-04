using Microsoft.AspNetCore.Identity;
using ResumeSystemManagement.Core.Entities;

namespace ResumeSystemManagement.Infrastructure.IdentityEntities;

public class User: IdentityUser
{
    public List<Resume> Resumes { get; } = new();
    public List<UserProject> Projects { get; } = new();
    public List<RecruterLike> RecruterLikes { get; } = new();
    public List<ChatHistory> ChatHistories { get; } = new();
}