using System.ComponentModel.DataAnnotations;

namespace ResumeSystemManagement.Core.Entities;

public class AttributeLibrary(string title, string description, int categoryId, int typeId, bool isBuiltIn = false)
{
    public int Id { get; init; }
    public int TypeId { get; set; } = typeId;
    public int CategoryId { get; set; } = categoryId;
    [MaxLength(100)]
    public string Title { get; private set; } = title;
    [MaxLength(256)]
    public string Description { get; private set; } = description;
    public bool IsBuiltIn { get; private set; } = isBuiltIn;
    public uint Version { get; set; }

    public AttributeCategory AttributeCategory { get; set; } = null!;
    public AttributeType AttributeType { get; set; } = null!;
    public List<PositionAttributeLibrary> PositionAttributeLibraries { get; } = new();
    public List<AttributeValueForList> AttributeValueForLists { get; init; } = new();
    public List<AttributeFilter> AttributeFilters { get; } = new();
    public List<CandidateAttributeValue> CandidateAttributeValues { get; } = new();
    
    public void SetType(int typeId) => TypeId = typeId;
    public void SetCategoryId(int categoryId) => CategoryId = categoryId;
    public void SetTitle(string title) => Title = title;
    public void SetDescription(string description) => Description = description;
}