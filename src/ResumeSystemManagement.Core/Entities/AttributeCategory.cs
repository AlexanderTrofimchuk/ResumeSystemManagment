namespace ResumeSystemManagement.Core.Entities;

public class AttributeCategory
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;

    public List<AttributeLibrary> AttributeLibraries { get; } = new();
}