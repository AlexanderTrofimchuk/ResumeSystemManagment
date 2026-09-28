namespace ResumeSystemManagement.Application.DTOs.Positions;

public class PopularPosition
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public int SubmittedResumes { get; set; }
}