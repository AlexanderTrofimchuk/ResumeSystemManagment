using ResumeSystemManagement.Application.DTOs.Attributes;
using ResumeSystemManagement.Core.Entities;
using ResumeSystemManagement.Core.ReadModels;

namespace ResumeSystemManagement.Application.Mappers;

public static class AttributeCategoryMapper
{
    public static AttributeCategoryDto ToAttributeCategoryDto(this AttributeCategory attributeCategory) =>
        new(attributeCategory.Id, attributeCategory.Title);
}

public static class AttributeTypeMapper
{
    public static AttributeTypeDto ToAttributeTypeDto(this AttributeType attributeType) =>
        new(attributeType.Id, attributeType.Title);
}

public static class AttributeLibraryMapper
{
    public static AttributeLibrary ToAttributeLibrary(this CreateAttributeDto dto) =>
        new(dto.Title, dto.Description, dto.CategoryId, dto.TypeId, dto.IsBuiltIn);

    public static AttributeLibrary ToAttributeLibrary(this EditAttributeDto dto)
    {
        var attribute = new AttributeLibrary(dto.Title, dto.Description, dto.TypeId, dto.CategoryId, dto.IsBuiltIn)
        {
            Id = dto.Id
        };

        if (dto.DropdownOptions is not null && dto.DropdownOptions.Count > 0)
        {
            attribute.SetAttributeValueForLists([
                .. dto.DropdownOptions
                    .Select(d => new AttributeValueForList(d.Value, dto.Id, d.Id))
            ]);
        }

        return attribute;
    }

    public static EditAttributeDto ToEditAttributeDto(this AttributeLibrary attribute) =>
        new(
            attribute.Id,
            attribute.TypeId,
            attribute.CategoryId,
            attribute.Title,
            attribute.Description,
            attribute.IsBuiltIn,
            [.. attribute.AttributeValueForLists.Select(a => new DropdownOption(a.Id, a.Value))]);

    public static List<AttributeValueForList> ToValueForList(this CreateAttributeDto dto, int id)
    {
        return dto.DropdownOptions != null ? 
            [.. dto.DropdownOptions.Select(o => new AttributeValueForList(o, id))] 
            : [];
    }

    public static AttributeDetail ToAttributeDetail(this AttributeLibrary attribute) =>
        new(attribute.Id, attribute.AttributeType.Title, attribute.AttributeCategory.Title, attribute.Title,
            attribute.Description, attribute.IsBuiltIn);
}
