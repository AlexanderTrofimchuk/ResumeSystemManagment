using ResumeSystemManagement.Core.Entities;

namespace ResumeSystemManagement.Application.DTOs.Attributes.Mapper;

public static class AttributeLibraryMapper
{
    public static AttributeLibrary ToAttributeLibrary(this CreateAttributeDto dto) => 
        new()
        {
            TypeId = dto.TypeId,
            CategoryId = dto.CategoryId,
            Title = dto.Title,
            Description = dto.Description,
            IsBuiltIn = dto.IsBuiltIn
        };

    public static AttributeLibrary ToAttributeLibrary(this EditAttributeDTo dto) => 
        new()
        {
            Id = dto.Id,
            TypeId = dto.TypeId,
            CategoryId = dto.CategoryId,
            Title = dto.Title,
            Description = dto.Description,
            IsBuiltIn = dto.IsBuiltIn,
            AttributeValueForLists =  dto.DropdownOptions!.Select(d => new AttributeValueForList{Id = d.Id,  Value = d.Value}).ToList()
        };

    public static EditAttributeDTo ToEditAttributeDTo(this AttributeLibrary attribute) =>
        new(attribute.Id, 
            attribute.TypeId,
            attribute.CategoryId, 
            attribute.Title, 
            attribute.Description, 
            attribute.IsBuiltIn,
            attribute.
                AttributeValueForLists.Select(a => new DropDownOption(a.Id,a.Value)).ToList());

    public static List<AttributeValueForList> ToValueForList(this CreateAttributeDto dto, int id) =>
        dto.DropDownOptions!.Select(o => new AttributeValueForList() { AttributeId = id, Value = o})
            .ToList();
}