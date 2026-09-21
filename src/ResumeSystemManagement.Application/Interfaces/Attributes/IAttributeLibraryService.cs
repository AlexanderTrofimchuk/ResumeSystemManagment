using FluentResults;
using ResumeSystemManagement.Application.DTOs.Attribute;
using ResumeSystemManagement.Core.Entities;

namespace ResumeSystemManagement.Application.Interfaces.Attributes;

public interface IAttributeLibraryService
{
    Task<Result<AttributeLibrary>> GetAttributeLibrary(int attributeId);
    Task<Result<EditAttributeDTo>> GetEditAttributeAsync(int id);
    Task<Result<AttributeDetails>> GetAttributesByNameAsync(string name);
    Task<Result<AttributeDetails>> GetAttributesAsync(int pageSize, int page);
    Task<Result<int>> CreateAttributeAsync(CreateAttributeDto dto);
    Task<Result<bool>> AddDropDownOptions(int id, CreateAttributeDto dto);
    Task<Result> EditAttributeAsync(EditAttributeDTo dTo);
    Task<Result> DeleteAttributeAsync(int id);
    Task<Result> BulkDeleteAttributesAsync(List<int> ids);
}