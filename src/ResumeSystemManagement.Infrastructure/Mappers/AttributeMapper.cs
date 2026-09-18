using ResumeSystemManagement.Core.Entities;
using ResumeSystemManagement.Core.ReadModels;

namespace ResumeSystemManagement.Infrastructure.Mappers;

public static class AttributeMapper
{
    public static AttributeDetail ToAttributeDetail(this AttributeLibrary attribute)
    {
        return new AttributeDetail(
            attribute.Id,
            attribute.AttributeType.Title,
            attribute.AttributeCategory.Title,
            attribute.Title,
            attribute.Description,
            attribute.IsBuiltIn
            );
    }
}