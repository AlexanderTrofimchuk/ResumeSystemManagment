namespace ResumeSystemManagement.Core.Entities;

public class CandidateAttributeValue
{
    public int Id { get; set; }
    public string UserId { get; set; } = null!;
    public int AttributeId { get; set; }
    public string Value { get; set; } = null!;

    public Resume Resume { get; set; } = null!;
    public AttributeLibrary Attribute { get; set; } = null!;
}