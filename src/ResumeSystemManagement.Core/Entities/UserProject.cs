namespace ResumeSystemManagement.Core.Entities;

public class UserProject
{
    public int Id { get; set; }
    public string UserId { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime StartProject { get; set; }
    public DateTime EndProject { get; set; }
    
}