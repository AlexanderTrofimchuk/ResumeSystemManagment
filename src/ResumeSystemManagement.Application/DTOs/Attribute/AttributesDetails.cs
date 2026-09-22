using ResumeSystemManagement.Core.ReadModels;

namespace ResumeSystemManagement.Application.DTOs.Attribute;

public class AttributeDetails
{
    public List<AttributeDetail> Attributes { get; set; } = new();
    public List<int> SelectIds { get; set; } = new List<int>();
    public CreateAttributeDto CreateAttributeDto { get; set; } = null!;
}