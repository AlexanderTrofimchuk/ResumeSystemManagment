namespace ResumeSystemManagement.Core.Entities;

public class RecruiterLike
{
    public int Id { get; set; }
    public int ResumeId { get; set; }
    public string RecruiterId { get; set; } = null!;
    public DateTime SetAt { get; set; } = DateTime.UtcNow;

    public Resume Resume { get; set; } = null!;
}