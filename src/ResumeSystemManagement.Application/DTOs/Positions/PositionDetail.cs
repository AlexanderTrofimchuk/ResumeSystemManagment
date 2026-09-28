using ResumeSystemManagement.Core.Enums;

namespace ResumeSystemManagement.Application.DTOs.Positions;

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
    