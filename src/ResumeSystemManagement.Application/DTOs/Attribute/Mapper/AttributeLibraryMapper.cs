using ResumeSystemManagement.Core.Entities;

namespace ResumeSystemManagement.Application.DTOs.Attribute.Mapper;

public static class AttributeLibraryMapper
{
    public static AttributeLibrary ToAttributeLibrary(this CreateAttributeDto dto) => 
        new(
            dto.Title,
            dto.Description,
            dto.CategoryId,
            dto.TypeId,
            dto.IsBuiltIn
        );

    public static AttributeLibrary ToAttributeLibrary(this EditAttributeDTo dto)
    {
        var attribute = new AttributeLibrary( dto.Title, dto.Description, dto.TypeId,dto.CategoryId,dto.IsBuiltIn)
        {
            Id = dto.Id
        };
        if (dto.DropdownOptions is not null && dto.DropdownOptions.Count > 0)
            attribute.SetAttributeValueForLists(dto.DropdownOptions!
                .Select(d => new AttributeValueForList(d.Value, dto.Id, d.Id)).ToList());
        
        return attribute;
    }

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