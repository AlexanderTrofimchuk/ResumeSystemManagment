using ResumeSystemManagement.Core.Entities;

namespace ResumeSystemManagement.Application.DTOs.Attribute.Mapper;

public static class AttributeCategoryMapper
{
    public static AttributeCategoryDTo AttributeCategoryDTo(this AttributeCategory attributeCategory)
    {
        return new(attributeCategory.Id, attributeCategory.Title);
    }
}