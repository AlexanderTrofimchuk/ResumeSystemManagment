using ResumeSystemManagement.Core.Entities;

namespace ResumeSystemManagement.Application.DTOs.Attributes.Mapper;

public static class AttributeTypeMapper
{
    public static AttributeTypeDTo ToAttributeTypeDTo(this AttributeType attributeType)
    {
        return new(attributeType.Id, attributeType.Title);
    }
}