namespace ResumeSystemManagement.Core.Entities;

public class ResumeAttributeValue
{
    public int Id { get; set; }
    public int ResumeId { get; set; }
    public int AttributeId { get; set; }
    public string Value { get; set; } = null!;

    public Resume Resume { get; set; } = null!;
    public AttributeLibrary Attribute { get; set; } = null!;
}