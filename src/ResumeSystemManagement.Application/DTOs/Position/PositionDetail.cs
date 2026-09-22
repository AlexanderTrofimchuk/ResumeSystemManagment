using ResumeSystemManagement.Core.Enums;

namespace ResumeSystemManagement.Application.DTOs.Position;

public record PositionDetail(
    int Id,
    string Title,
    string Description,
    PositionPermissions Permissions,
    DateTime CreatedOn,
    int MaxProjects);
    
public static class PositionDetailMapper
{
    public static PositionDetail ToPositionDetail(this Core.Entities.Position position) =>
        new(position.Id, 
            position.Title, 
            position.Description,
            position.Permissions, 
            position.CreateAt, 
            position.MaxProjects);
            

    public static Core.Entities.Position ToPosition(this PositionDetail detail, string createBy) =>
        new(detail.Title,detail.Description,createBy,permissions:detail.Permissions)
        {
            Id = detail.Id,
            CreateAt =  detail.CreatedOn
        };
}