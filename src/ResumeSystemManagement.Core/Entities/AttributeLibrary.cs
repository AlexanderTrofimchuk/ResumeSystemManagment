namespace ResumeSystemManagement.Core.Entities;

public class AttributeLibrary
{
    public int Id { get; set; }
    public int TypeId { get; set; }
    public int CategoryId { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public bool IsBuiltIn { get; set; } = false;
    public uint Version { get; set; }

    public AttributeCategory AttributeCategory { get; set; } = null!;
    public AttributeType AttributeType { get; set; } = null!;
    public List<Position> Positions { get; } = new();
    public List<AttributeValueForList> AttributeValueForLists { get; init; } = new();
    public List<AttributeFilter> AttributeFilters { get; } = new();
    public List<CandidateAttributeValue> CandidateAttributeValues { get; } = new();
}