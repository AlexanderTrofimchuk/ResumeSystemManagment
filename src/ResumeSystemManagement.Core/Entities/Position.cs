using System.ComponentModel.DataAnnotations;
using ResumeSystemManagement.Core.Enums;

namespace ResumeSystemManagement.Core.Entities;

public class Position(string title, string description, string createdBy,DateTime? closedAt = null,PositionPermissions permissions = PositionPermissions.Public)
{
    public int Id { get; init; }
    [MaxLength(100)]
    public string Title { get; private set; } = title;
    [MaxLength(1000)]
    public string Description { get; private set; } = description;
    public PositionPermissions Permissions { get; private set; } = permissions;
    public int MaxProjects { get; private set; }
    public uint Version { get; set; }
    public string CreatedBy { get; private set; } = createdBy;
    public DateTime CreateAt { get; init; } = DateTime.UtcNow;
    public DateTime? ClosedAt { get; private set; } = closedAt;

    public List<Resume> Resumes { get; } = new();
    public List<AttributeFilter> AttributeFilters { get; } = new();
    public List<PositionAttributeLibrary> PositionAttributeLibraries { get; } = new();
    public List<Discussion> Discussions { get; } = new();
    public List<Tag> RequiredTags { get; } = new();
    
    public void SetMaxProjects(int maxProjects)
    {
        MaxProjects = maxProjects;
    }
}