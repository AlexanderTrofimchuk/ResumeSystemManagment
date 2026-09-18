using ResumeSystemManagement.Core.Enums;

namespace ResumeSystemManagement.Application.DTOs.Position;

public record PositionDetail(
    int Id,
    string Title,
    string Description,
    PositionPermissions Permissions,
    DateTime CreatedOn);
    
public static class PositionDetailMapper
{
    public static PositionDetail ToPositionDetail(this Core.Entities.Position position) =>
        new(position.Id, position.Title, position.Description,
            position.Permissions, position.CreateAt);

    public static Core.Entities.Position ToPosition(this PositionDetail detail) =>
        new()
        {
            Id = detail.Id,
            Title = detail.Title,
            Description = detail.Description,
            Permissions = detail.Permissions,
            CreateAt =  detail.CreatedOn
        };
}