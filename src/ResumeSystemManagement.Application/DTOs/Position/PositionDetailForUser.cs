using ResumeSystemManagement.Application.DTOs.Attribute;
using ResumeSystemManagement.Application.DTOs.PositionTemplate;
using ResumeSystemManagement.Core.Enums;

namespace ResumeSystemManagement.Application.DTOs.Position;

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

public static class PositionDetailForUserExtensions
{
    public static PositionDetailForUser MapToPositionForUser(this Core.Entities.Position position)
    {
        return new PositionDetailForUser
        {
            PositionId = position.Id,
            PositionTitle = position.Title,
            Desciption = position.ShortDescription,
            RequiredProjectCount = position.MaxProjects,
            AttributeBySection = position.PositionAttributeLibraries
                .GroupBy(p => p.Section)
                .Select(g => new AttributeSection
                {
                    Section = g.Key,
                    AttributeTemplates = g.Select(p => p.AttributeLibrary.ToDetailView()).ToList()
                })
                .ToList()
        };
    }
}
