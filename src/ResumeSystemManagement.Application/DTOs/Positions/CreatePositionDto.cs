using System.ComponentModel.DataAnnotations;
using ResumeSystemManagement.Core.Enums;

namespace ResumeSystemManagement.Application.DTOs.Positions;

public record CreatePositionDto(
    [MaxLength(100)]
    string Title,
    [MaxLength(1000)]
    string Description,
    int MaxProjects,
    PositionLevel Level,
    PositionPermissions Permissions = PositionPermissions.Public);
    
