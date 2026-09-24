using ResumeSystemManagement.Core.Enums;

namespace ResumeSystemManagement.Core.Entities;

public class Resume
{
    public int Id { get; set; }
    public int PositionId { get; set; }
    public string UserId { get; set; } = null!;
    public PublishStatus Status { get; set; } = PublishStatus.Draft;
    
    public Position Position { get; set; } = null!;
    public List<RecruiterLike> Likes { get; } = new();
}