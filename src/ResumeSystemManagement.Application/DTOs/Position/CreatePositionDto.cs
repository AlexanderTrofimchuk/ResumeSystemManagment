using System.ComponentModel.DataAnnotations;
using ResumeSystemManagement.Core.Enums;

namespace ResumeSystemManagement.Application.DTOs.Position;

public record CreatePositionDto(
    [MaxLength(100)]
    string Title,
    [MaxLength(1000)]
    string Description,
    int MaxProjects,
    PositionPermissions Permissions = PositionPermissions.Public);
    

public static class CreatePositionMapper
{
    public static Core.Entities.Position MapToPosition(this CreatePositionDto dto,string createBy)
    {
        var newPosition = new Core.Entities.Position(dto.Title,
                dto.Description,
                createBy,
                permissions: dto.Permissions)
            ;
        newPosition.SetMaxProjects(dto.MaxProjects);
        return newPosition;
    }
        
}