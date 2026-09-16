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
            IsBuiltIn = dto.IsBuiltIn
        };

    public static EditAttributeDTo ToEditAttributeDTo(this AttributeLibrary attribute) =>
        new(attribute.Id, 
            attribute.TypeId,
            attribute.CategoryId, 
            attribute.Title, 
            attribute.Description, 
            attribute.IsBuiltIn);
}