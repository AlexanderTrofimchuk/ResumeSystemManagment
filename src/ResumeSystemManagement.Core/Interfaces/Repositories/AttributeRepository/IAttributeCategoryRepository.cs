using ResumeSystemManagement.Core.Entities;

namespace ResumeSystemManagement.Core.Interfaces.Repositories.AttributeRepository;

public interface IAttributeCategoryRepository
{
    Task<List<AttributeCategory>> GetAttributeCategories();
}