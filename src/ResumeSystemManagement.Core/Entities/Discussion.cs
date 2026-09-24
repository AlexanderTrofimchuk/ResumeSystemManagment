using System.ComponentModel.DataAnnotations;

namespace ResumeSystemManagement.Core.Entities;

public class Discussion(int positionId, string text, string createBy)
{
    public int Id { get; init; }
    public int PositionId { get; private set; } = positionId;
    [MaxLength(500)]
    public string Text { get; private set; } = text;
    public DateTime SendAt { get; init; } = DateTime.UtcNow;
    public string CreateBy { get; private set; } = createBy;

    public Position Position { get; set; } = null!;
}