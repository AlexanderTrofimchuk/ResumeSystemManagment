using ResumeSystemManagement.Core.Entities;

namespace ResumeSystemManagement.Application.DTOs.Attribute.Mapper;

public static class AttributeLibraryMapper
{
    public static AttributeLibrary ToAttributeLibrary(this CreateAttributeDto dto) => 
        new(
            dto.Title,
            dto.Description,
            dto.TypeId,
            dto.CategoryId,
            dto.IsBuiltIn
        );

    public static AttributeLibrary ToAttributeLibrary(this EditAttributeDTo dto) => 
        new( dto.Title, dto.Description, dto.TypeId,dto.CategoryId,dto.IsBuiltIn)
        {
            Id = dto.Id,
            AttributeValueForLists =  dto.DropdownOptions!.Select(d => new AttributeValueForList(d.Value,dto.Id,d.Id)).ToList()
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
        dto.DropDownOptions!.Select(o => new AttributeValueForList(o,id))
            .ToList();
}