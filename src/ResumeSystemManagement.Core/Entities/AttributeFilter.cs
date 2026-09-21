using System.ComponentModel.DataAnnotations;

namespace ResumeSystemManagement.Core.Entities;

public class AttributeFilter(int positionId, int attributeId, string @operator, string value)
{
    public int Id { get; init; }
    public int PositionId { get; private set; } =  positionId;
    public int AttributeId { get; private set; } =  attributeId;
    [MaxLength(1)]
    public string Operator { get; private set; } = @operator;
    public string Value { get; private set; } = value;

    public Position Position { get; set; } = null!;
    public AttributeLibrary Attribute { get; set; } = null!;
}