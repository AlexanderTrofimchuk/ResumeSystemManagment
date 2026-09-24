using ResumeSystemManagement.Core.Enums;

namespace ResumeSystemManagement.Application.DTOs.Position;

public record PositionDetail(
    int Id,
    string Title,
    string Description,
    PositionPermissions Permissions,
    PositionLevel Level,
    PublishStatus Status,
    int MaxProjects,
    DateTime CreatedOn,
    DateTime? ClosedAt
    );
    
public static class PositionDetailMapper
{
    public static PositionDetail ToPositionDetail(this Core.Entities.Position position) =>
        new(position.Id, 
            position.Title, 
            position.ShortDescription,
            position.Permissions,
            position.Level,
            position.PublishStatus,
            position.MaxProjects,
            position.CreateAt,
            position.ClosedAt);
            

    public static Core.Entities.Position ToPosition(this PositionDetail detail, string createBy)
    {
        Core.Entities.Position position = new(detail.Title, detail.Description, createBy, permissions: detail.Permissions)
        {
            Id = detail.Id,
            CreateAt = detail.CreatedOn,
        };
        position.SetMaxProjects(detail.MaxProjects);
        position.SetLevel(detail.Level);
        position.SetPublishStatus(detail.Status);
        return position;
    }
}