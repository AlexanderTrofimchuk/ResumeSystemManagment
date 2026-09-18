using ResumeSystemManagement.Core.Enums;

namespace ResumeSystemManagement.Application.DTOs.Position;

public record CreatePositionDto(
    string Title,
    string Description,
    string CreatedBy,
    PositionPermissions Permissions = PositionPermissions.Public);
    

public static class CreatePositionMapper
{
    public static Core.Entities.Position MapToPosition(this CreatePositionDto dto,string createdBy)=>
        new()
        {
          Title = dto.Title,
          Description = dto.Description,
          Permissions = dto.Permissions,
          CreatedBy = createdBy
        };
}