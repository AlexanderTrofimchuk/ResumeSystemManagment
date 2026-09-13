namespace ResumeSystemManagement.Core.Entities;

public class AttributeFilter
{
    public int Id { get; set; }
    public int PositionId { get; set; }
    public int AttributeId { get; set; }
    public string Operator { get; set; } = null!;
    public string Value { get; set; } = null!;

    public Position Position { get; set; } = null!;
    public AttributeLibrary Attribute { get; set; } = null!;
    public List<CandidateAttributeValue> ResumeAttributeValues { get; } = new();
}