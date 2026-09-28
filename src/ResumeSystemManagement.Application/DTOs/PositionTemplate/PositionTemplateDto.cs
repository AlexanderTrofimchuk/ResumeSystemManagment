using ResumeSystemManagement.Application.DTOs.Attributes;
using ResumeSystemManagement.Core.Enums;

namespace ResumeSystemManagement.Application.DTOs.PositionTemplate;

public class PositionTemplateDto
{
    public int PositionId { get; set; }
    public string PositionTitle { get; set; } = null!;
    public PublishStatus  PublishStatus { get; set; }
    public List<AttributeSection> AttributeInPosition { get; set; } = new ();
    
    public List<AttributeTemplate> GetAttributes(TemplateSection section) =>
        AttributeInPosition
            .FirstOrDefault(s => s.Section == section)
            ?.AttributeTemplates ?? new();
}
