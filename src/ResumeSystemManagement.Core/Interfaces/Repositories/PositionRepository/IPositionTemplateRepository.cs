using ResumeSystemManagement.Core.Entities;
using ResumeSystemManagement.Core.Enums;

namespace ResumeSystemManagement.Core.Interfaces.Repositories.PositionRepository;

public interface IPositionTemplateRepository
{
    Task<Position?> GetByIdAsync(int id);
    Task<HashSet<int>>  GetAttributeInPosition(int positionId);
    Task AttachAttributeAsync(int positionId, int attributeId, TemplateSection section);
    Task Update(int positionId, int attributeId, TemplateSection section);
    Task Delete(int positionId, int attributeId);
}