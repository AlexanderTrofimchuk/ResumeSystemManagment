using ResumeSystemManagement.Core.Enums;

namespace ResumeSystemManagement.Core.Entities;

public class PositionAttributeLibrary
{
    public int PositionId { get; set; }
    public Position Position { get; set; } = null!;

    public int AttributeLibraryId { get; set; }
    public AttributeLibrary AttributeLibrary { get; set; } = null!;

    public TemplateSection Section { get; set; }
}