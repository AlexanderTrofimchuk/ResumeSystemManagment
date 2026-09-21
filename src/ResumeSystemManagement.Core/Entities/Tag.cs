using System.ComponentModel.DataAnnotations;

namespace ResumeSystemManagement.Core.Entities;

public class Tag(string name)
{
    public int Id { get; init; }
    
    [MaxLength(100)]
    public string Name { get; private set; } = name;

    public List<UserProject> UserProjects { get; set; } = new();
    public List<Position> Positions { get; set; } = new();
}