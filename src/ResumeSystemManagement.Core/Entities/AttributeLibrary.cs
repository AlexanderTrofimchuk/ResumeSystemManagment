namespace ResumeSystemManagement.Core.Entities;

public class AttributeLibrary
{
    public int Id { get; set; }
    public int TypeId { get; set; }
    public int CategoryId { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public bool IsBuiltIn { get; set; } = false;

    public AttributeCategory AttributeCategory { get; set; } = null!;
    public AttributeType AttributeType { get; set; } = null!;
    public List<AttributeValueForList> AttributeValueForLists { get; } = new();
    public List<AttributeFilter> AttributeFilters { get; } = new();
    public List<ResumeAttributeValue> ResumeAttributeValues { get; } = new();
}