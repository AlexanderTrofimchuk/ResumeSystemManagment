using ResumeSystemManagement.Application.DTOs.Attribute;
using ResumeSystemManagement.Core.Enums;
using ResumeSystemManagement.Core.ReadModels;

namespace ResumeSystemManagement.Application.DTOs.PositionTemplate;

public class PositionTemplateDTo
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

public static class PositionTemplateDToExtensions
{
    public static PositionTemplateDTo MapToPositionTemplate(this Core.Entities.Position position){
        return new()
        {
            PositionId = position.Id,
            PositionTitle = position.Title,
            PublishStatus = position.PublishStatus,
            AttributeInPosition = position.PositionAttributeLibraries
                .GroupBy(p => p.Section)
                .Select(g => new AttributeSection
                {
                    Section = g.Key,
                    AttributeTemplates = g.Select(p => p.AttributeLibrary.ToAttributeTemplate()).ToList()
                })
                .ToList()
        };
    }
}