using ResumeSystemManagement.Core.Enums;

namespace ResumeSystemManagement.Application.DTOs.Position;

public record EditPositionDto(
    int Id,
    string Title,
    string Description,
    PositionPermissions Permissions,
    DateTime CreatedOn,
    uint Version,
    string CreatedBy,
    int MaxProject);

public static class EditPositionMapper
{
    public static Core.Entities.Position MapToPosition(this EditPositionDto dto) =>
        new(dto.Title,dto.Description,dto.CreatedBy,permissions: dto.Permissions)
        {
            Id = dto.Id,
            Version = dto.Version,
        };

    public static EditPositionDto MapToEditPosition(this Core.Entities.Position position) =>
        new(position.Id, position.Title, position.Description, position.Permissions, position.CreateAt,  position.Version, position.CreatedBy, position.MaxProjects);
}