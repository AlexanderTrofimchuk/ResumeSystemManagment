using ResumeSystemManagement.Core.ReadModels;

namespace ResumeSystemManagement.Application.DTOs.Attributes;

public class AttributeDetails
{
    public List<AttributeDetail> Attributes { get; set; }
    public List<int> SelectIds { get; set; } = new List<int>();
    public CreateAttributeDto  CreateAttributeDto { get; set; }
}