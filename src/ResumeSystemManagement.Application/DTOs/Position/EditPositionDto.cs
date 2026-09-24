using Microsoft.VisualBasic;
using ResumeSystemManagement.Core.Enums;

namespace ResumeSystemManagement.Application.DTOs.Position;

public record EditPositionDto(
    int Id,
    string Title,
    string Description,
    PositionPermissions Permissions,
    DateTime CreatedOn,
    uint Version,
    PositionLevel Level,
    string CreatedBy,
    int MaxProject);

public static class EditPositionMapper
{
    public static Core.Entities.Position MapToPosition(this EditPositionDto dto)
    {
        Core.Entities.Position position = new(
            dto.Title,
            dto.Description,
            dto.CreatedBy,
            DateTime.SpecifyKind(dto.CreatedOn, DateTimeKind.Utc),
            permissions: dto.Permissions)
        {
            Id = dto.Id,
            Version = dto.Version,
        };
        position.SetMaxProjects(dto.MaxProject);
        position.SetLevel(dto.Level);
        position.SetModifiedAt(DateTime.UtcNow);
        return position;
    }

    public static EditPositionDto MapToEditPosition(this Core.Entities.Position position) =>
        new(position.Id, 
            position.Title, 
            position.ShortDescription, 
            position.Permissions, 
            position.CreateAt,  
            position.Version, 
            position.Level,
            position.CreatedBy, 
            position.MaxProjects);
}