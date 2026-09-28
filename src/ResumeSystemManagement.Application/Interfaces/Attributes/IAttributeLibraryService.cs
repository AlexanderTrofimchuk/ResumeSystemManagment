using FluentResults;
using ResumeSystemManagement.Application.DTOs.Attributes;
using ResumeSystemManagement.Core.Entities;

namespace ResumeSystemManagement.Application.Interfaces.Attributes;

public interface IAttributeLibraryService
{
    Task<Result<AttributeLibrary>> GetAttributeLibrary(int attributeId);
    Task<Result<EditAttributeDto>> GetEditAttributeAsync(int id);
    Task<Result<AttributeDetails>> GetAttributesByNameAsync(string name, int page);
    Task<Result<AttributeDetails>> GetAttributesAsync(int page);
    Task<Result<int>> CreateAttributeAsync(CreateAttributeDto dto);
    Task<Result<bool>> AddDropdownOptions(int id, CreateAttributeDto dto);
    Task<Result> EditAttributeAsync(EditAttributeDto dto);
    Task<Result> DeleteAttributeAsync(int id);
    Task<Result> BulkDeleteAttributesAsync(List<int> ids);
}