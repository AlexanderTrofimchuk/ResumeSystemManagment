using ResumeSystemManagement.Application.DTOs.Attributes;
using ResumeSystemManagement.Application.DTOs.PositionTemplate;
using ResumeSystemManagement.Core.Enums;

namespace ResumeSystemManagement.Application.DTOs.Positions;

public class PositionDetailForUser
{
    public int PositionId { get; set; }
    public string PositionTitle { get; set; } = null!;
    public string Desciption { get; set; } = null!;
    public int RequiredProjectCount { get; set; }
    public List<AttributeSection> AttributeBySection { get; set; } = new ();
    
    public List<AttributeTemplate> GetAttributes(TemplateSection section) =>
        AttributeBySection
            .FirstOrDefault(s => s.Section == section)
            ?.AttributeTemplates ?? new();
}
