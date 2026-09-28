using FluentResults;
using ResumeSystemManagement.Core.Entities;
using ResumeSystemManagement.Core.ReadModels;

namespace ResumeSystemManagement.Core.Interfaces.Repositories.AttributeRepository;

public interface IAttributeLibraryRepository
{
    Task<AttributeLibrary?> GetAttributeByIdAsync(int id);
    Task<List<AttributeLibrary>> GetByCategoryAsync(int categoryId);
    Task<List<AttributeDetail>> GetByNameTypeAsync(string name, int page,int typeId);
    Task<int> GetTotalCountAsync();
    Task<int> GetByNameCountAsync(string name);
    Task<List<AttributeDetail>> GetByNameAsync(string name, int page);
    Task<List<AttributeDetail>> GetAttributesAsync(int page);
    Task<List<AttributeLibrary>> GetBuildInAttribute();
    Task<int> CreateAttributeAsync(AttributeLibrary attribute);
    Task<bool> CreateAttributeListValue(List<AttributeValueForList> variations);
    Task<bool> UpdateAttributeAsync(AttributeLibrary attribute);
    Task<bool> DeleteAttributeAsync(AttributeLibrary attribute);
    Task<Result<bool>> BulkDeleteAttributesAsync(List<int> ids);
}