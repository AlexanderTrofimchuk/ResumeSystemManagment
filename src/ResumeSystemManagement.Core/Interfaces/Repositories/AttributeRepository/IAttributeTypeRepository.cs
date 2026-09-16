using ResumeSystemManagement.Core.Entities;

namespace ResumeSystemManagement.Core.Interfaces.Repositories.AttributeRepository;

public interface IAttributeTypeRepository
{
    Task<List<AttributeType>> GetAttributeTypes();
}