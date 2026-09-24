using System.ComponentModel.DataAnnotations;
using ResumeSystemManagement.Core.Enums;

namespace ResumeSystemManagement.Core.Entities;

public class Position(string title, string shortDescription, string createdBy,DateTime? closedAt = null,PositionPermissions permissions = PositionPermissions.Public)
{
    public int Id { get; set; }
    [MaxLength(100)]
    public string Title { get; private set; } = title;
    [MaxLength(1000)]
    public string ShortDescription { get; private set; } = shortDescription;
    public PositionPermissions Permissions { get; private set; } = permissions;
    public int MaxProjects { get; private set; }
    public PositionLevel Level { get; private set; }
    public uint Version { get; set; }
    public string CreatedBy { get; private set; } = createdBy;
    public PublishStatus PublishStatus { get; private set; } = PublishStatus.Draft;
    public DateTime CreateAt { get; init; } = DateTime.UtcNow;
    public DateTime? ModifiedAt { get; private set; } = null; 
    public DateTime? ClosedAt { get; private set; } = closedAt;

    public List<Resume> Resumes { get; } = new();
    public List<AttributeFilter> AttributeFilters { get; } = new();
    public List<PositionAttributeLibrary> PositionAttributeLibraries { get; private set; } = new();
    public List<Discussion> Discussions { get; } = new();
    public List<Tag> RequiredTags { get; } = new();
    
    public void SetMaxProjects(int maxProjects) => MaxProjects = maxProjects;
    public void SetLevel(PositionLevel level) => Level = level;
    public void SetPublishStatus(PublishStatus publishStatus) => PublishStatus = publishStatus;
    public void SetModifiedAt(DateTime modifiedAt) => ModifiedAt = modifiedAt;

    public void SetPositionAttributeLibraries(List<PositionAttributeLibrary> positionLibrary)
    {
        PositionAttributeLibraries =  positionLibrary;
    }
}