using ResumeSystemManagement.Core.Enums;

namespace ResumeSystemManagement.Application.DTOs.Position;

public record EditPositionDto(
    int Id,
    string Title,
    string Description,
    PositionPermissions Permissions,
    DateTime CreatedOn,
    uint Version,
    string CreatedBy);

public static class EditPositionMapper
{
    public static Core.Entities.Position MapToPosition(this EditPositionDto dto) =>
        new()
        {
            Id = dto.Id,
            Title = dto.Title,
            Description = dto.Description,
            Permissions = dto.Permissions,
            CreateAt = DateTime.SpecifyKind(dto.CreatedOn, DateTimeKind.Utc),
            Version = dto.Version,
            CreatedBy = dto.CreatedBy
        };

    public static EditPositionDto MapToEditPosition(this Core.Entities.Position position) =>
        new(position.Id, position.Title, position.Description, position.Permissions, position.CreateAt,  position.Version, position.CreatedBy);
}