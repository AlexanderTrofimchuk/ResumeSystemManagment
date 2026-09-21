using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using ResumeSystemManagement.Core.Entities;

namespace ResumeSystemManagement.Infrastructure.IdentityEntities;

public class AppUser: IdentityUser
{
    [MaxLength(150)]
    public string FullName { get; set; } = null!;
    public uint ProfileVersion {get; set;}

    public List<Position> Positions { get; } = new();
    public List<Resume> Resumes { get; } = new();
    public List<UserProject> Projects { get; } = new();
    public List<RecruiterLike> RecruiterLikes { get; } = new();
    public List<Discussion> Discussions { get; } = new();
    public List<CandidateAttributeValue> CandidateAttributeValues { get; } = new ();
}