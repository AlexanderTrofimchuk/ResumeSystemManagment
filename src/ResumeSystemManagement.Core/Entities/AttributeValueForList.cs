namespace ResumeSystemManagement.Core.Entities;

public class AttributeValueForList(string value, int attributeId, int id = 0)
{
    public int Id { get; init; } = id;
    public int AttributeId { get; private set; } = attributeId;
    public string Value { get; private set; } = value;
    public uint Version { get; set; }
    public AttributeLibrary Attribute { get; set; } = null!;
}