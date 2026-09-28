using ResumeSystemManagement.Application.DTOs.Positions;
using ResumeSystemManagement.Application.DTOs.PositionTemplate;

namespace ResumeSystemManagement.Application.Mappers;

public static class PositionMapper
{
    public static PositionDetail ToPositionDetail(this Core.Entities.Position position) =>
        new(position.Id, position.Title, position.ShortDescription, position.Permissions, position.Level,
            position.PublishStatus, position.MaxProjects, position.CreateAt, position.ClosedAt);

    public static Core.Entities.Position ToPosition(this PositionDetail detail, string createBy)
    {
        var position = new Core.Entities.Position(detail.Title, detail.Description, createBy,
            permissions: detail.Permissions)
        {
            Id = detail.Id,
            CreateAt = detail.CreatedOn
        };
        position.SetMaxProjects(detail.MaxProjects);
        position.SetLevel(detail.Level);
        position.SetPublishStatus(detail.Status);
        return position;
    }

    public static Core.Entities.Position ToPosition(this CreatePositionDto dto, string createBy)
    {
        var position = new Core.Entities.Position(dto.Title, dto.Description, createBy,
            permissions: dto.Permissions);
        position.SetMaxProjects(dto.MaxProjects);
        position.SetLevel(dto.Level);
        return position;
    }

    public static Core.Entities.Position ToPosition(this EditPositionDto dto)
    {
        var position = new Core.Entities.Position(dto.Title, dto.Description, dto.CreatedBy,
            DateTime.SpecifyKind(dto.CreatedOn, DateTimeKind.Utc), permissions: dto.Permissions)
        {
            Id = dto.Id,
            Version = dto.Version
        };
        position.SetMaxProjects(dto.MaxProject);
        position.SetLevel(dto.Level);
        position.SetModifiedAt(DateTime.UtcNow);
        return position;
    }

    public static EditPositionDto ToEditPositionDto(this Core.Entities.Position position) =>
        new(position.Id, position.Title, position.ShortDescription, position.Permissions, position.CreateAt,
            position.Version, position.Level, position.CreatedBy, position.MaxProjects);

    public static PositionDetailForUser ToPositionForUser(this Core.Entities.Position position) =>
        new()
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
                    AttributeTemplates = [.. g.Select(p => p.AttributeLibrary.ToAttributeTemplateDetails())]
                }).ToList()
        };

    public static PositionTemplateDto ToPositionTemplate(this Core.Entities.Position position) =>
        new()
        {
            PositionId = position.Id,
            PositionTitle = position.Title,
            PublishStatus = position.PublishStatus,
            AttributeInPosition = position.PositionAttributeLibraries
                .GroupBy(p => p.Section)
                .Select(g => new AttributeSection
                {
                    Section = g.Key,
                    AttributeTemplates = g.Select(p => p.AttributeLibrary.ToAttributeTemplateDetails()).ToList()
                })
                .ToList()
        };
}
