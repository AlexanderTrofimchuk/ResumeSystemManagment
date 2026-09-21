using System.ComponentModel.DataAnnotations;

namespace ResumeSystemManagement.Core.Entities;

public class UserProject(string name, string userId, string description, DateTime startProject, DateTime? endProject = null)
{
    public int Id { get; init; }
    [MaxLength(150)]
    public string Name { get; private set; } = name;
    public string UserId { get; private set; } = userId;
    [MaxLength(400)]
    public string Description { get; private set; } = description;
    public DateTime StartProject { get; private set; } = startProject;
    public DateTime? EndProject { get; private set; } = endProject;
    public uint Version { get; set; }
    public List<Tag> Tags { get; set; } = new();
}