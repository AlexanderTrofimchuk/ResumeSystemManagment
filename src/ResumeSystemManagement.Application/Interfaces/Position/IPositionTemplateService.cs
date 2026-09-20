using FluentResults;
using ResumeSystemManagement.Application.DTOs.PositionTemplate;
using ResumeSystemManagement.Core.Enums;

namespace ResumeSystemManagement.Application.Interfaces.Position;

public interface IPositionTemplateService
{
    Task<Result<PositionTemplateDTo>> GetById(int id);
    Task<Result<HashSet<int>>> GetAttributeInPosition(int positionId);
    Task<Result> AssignAttributeAsync(int positionId, int attributeId, TemplateSection section);
    Task<Result> UpdateSectionAsync(int positionId, int attributeId, TemplateSection section);
    Task<Result> DeleteAttributeAsync(int positionId, int attributeId);
}