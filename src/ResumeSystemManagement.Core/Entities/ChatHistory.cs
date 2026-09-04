namespace ResumeSystemManagement.Core.Entities;

public class ChatHistory
{
    public int Id { get; set; }
    public int ResumeId { get; set; }
    public string Text { get; set; } = null!;
    public DateTime SendAt { get; set; } = DateTime.UtcNow;
    public string SentBy { get; set; } = null!;

    public Resume Resume { get; set; } = null!;
}