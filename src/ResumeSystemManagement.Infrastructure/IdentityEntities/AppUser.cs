using Microsoft.AspNetCore.Identity;
using ResumeSystemManagement.Core.Entities;

namespace ResumeSystemManagement.Infrastructure.IdentityEntities;

public class AppUser: IdentityUser
{
    public string FullName { get; set; } = null!;
    
    public List<Resume> Resumes { get; } = new();
    public List<UserProject> Projects { get; } = new();
    public List<RecruiterLike> RecruiterLikes { get; } = new();
    public List<ChatHistory> ChatHistories { get; } = new();
    public List<CandidateAttributeValue> CandidateAttributeValues { get; } = new ();
}