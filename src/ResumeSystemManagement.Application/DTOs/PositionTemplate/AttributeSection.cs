using ResumeSystemManagement.Application.DTOs.Attributes;
using ResumeSystemManagement.Core.Entities;
using ResumeSystemManagement.Core.Enums;

namespace ResumeSystemManagement.Application.DTOs.PositionTemplate;

public class AttributeSection
{
    public TemplateSection Section { get; set; }
    public List<AttributeTemplate> AttributeTemplates { get; set; } = new();
}