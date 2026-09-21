namespace ResumeSystemManagement.Core.Entities;

public class CandidateAttributeValue(string userId,int attributeId,string value)
{
    public int Id { get; init; }
    public string UserId { get; private set; } = userId;
    public int AttributeId { get; private set; } = attributeId;
    public string Value { get; private set; } = value;
    
    public AttributeLibrary Attribute { get; set; } = null!;
}