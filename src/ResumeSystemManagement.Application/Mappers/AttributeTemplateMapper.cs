using ResumeSystemManagement.Application.DTOs.Attributes;
using ResumeSystemManagement.Core.Entities;
using ResumeSystemManagement.Core.ReadModels;

namespace ResumeSystemManagement.Application.Mappers;

public static class AttributeTemplateMapper
{
    public static AttributeTemplate ToAttributeTemplate(this AttributeLibrary attributeLibrary) =>
        new(attributeLibrary.Id, attributeLibrary.Title, attributeLibrary.Description, attributeLibrary.AttributeType.Title);

    public static AttributeTemplate ToAttributeTemplateDetails(this AttributeLibrary attributeLibrary) =>
        new(attributeLibrary.Id, attributeLibrary.Title);

    public static AttributeTemplate AttributeDetailToTemplate(this AttributeDetail attributeDetail) =>
        new(attributeDetail.Id, attributeDetail.Title, attributeDetail.Description, attributeDetail.TypeName);
}
