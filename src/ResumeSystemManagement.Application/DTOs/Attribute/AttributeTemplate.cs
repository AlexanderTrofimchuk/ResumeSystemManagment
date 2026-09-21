using ResumeSystemManagement.Core.Entities;
using ResumeSystemManagement.Core.ReadModels;

namespace ResumeSystemManagement.Application.DTOs.Attribute;

public record AttributeTemplate(int Id, string Name, string Description, string TypeName);

public static class AttributeTemplateExtention
{
    public static AttributeTemplate ToAttributeTemplate(this AttributeLibrary attributeLibrary)
    {
        return new(attributeLibrary.Id, attributeLibrary.Title, attributeLibrary.Description, attributeLibrary.AttributeType.Title);
    }

    public static AttributeTemplate ToAttributeTemplate(this AttributeDetail attributeTemplate)
    {
        return new(
            attributeTemplate.Id,
            attributeTemplate.Title,
            attributeTemplate.Description,
            attributeTemplate.TypeName
        );
    }
}