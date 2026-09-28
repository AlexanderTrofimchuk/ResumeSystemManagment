using ResumeSystemManagement.Core.Enums;

namespace ResumeSystemManagement.Application.DTOs.Positions;

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
