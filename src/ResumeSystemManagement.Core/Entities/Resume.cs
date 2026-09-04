using ResumeSystemManagement.Core.Enums;

namespace ResumeSystemManagement.Core.Entities;

public class Resume
{
    public int Id { get; set; }
    public int PositionId { get; set; }
    public string UserId { get; set; } = null!;
    public ResumeStatus Status { get; set; } = ResumeStatus.NotPublish;
    
    public Position Position { get; set; } = null!;
    public List<RecruterLike> RecruterLikes { get; } = new();
    public List<ChatHistory> Histories { get; } = new();
    public List<RecruterLike> Likes { get; } = new();
}