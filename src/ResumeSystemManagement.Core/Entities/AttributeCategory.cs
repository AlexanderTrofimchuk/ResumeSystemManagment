using System.ComponentModel.DataAnnotations;

namespace ResumeSystemManagement.Core.Entities;

public class AttributeCategory(string title)
{
    public int Id { get; init; }
    
    [MaxLength(100)]
    public string Title { get; private set; } = title;

    public List<AttributeLibrary> AttributeLibraries { get; } = new();
}