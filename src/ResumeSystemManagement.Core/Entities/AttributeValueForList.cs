namespace ResumeSystemManagement.Core.Entities;

public class AttributeValueForList
{
    public int Id { get; set; }
    public int AttributeId { get; set; }
    public string Value { get; set; } = null!;

    public AttributeLibrary Attribute { get; set; } = null!;
}