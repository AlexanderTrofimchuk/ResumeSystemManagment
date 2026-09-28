using FluentResults;
using ResumeSystemManagement.Application.DTOs.Attributes;
using ResumeSystemManagement.Application.Interfaces.Attributes;
using ResumeSystemManagement.Application.Mappers;
using ResumeSystemManagement.Core.Interfaces.Repositories.AttributeRepository;

namespace ResumeSystemManagement.Application.Service.Attributes;

public class AttributeCategoryService(IAttributeCategoryRepository repository): IAttributeCategoryService
{
    public async Task<Result<List<AttributeCategoryDto>>> GetAttributeTypes()
    {
        var task = Result.Try(repository.GetAttributeCategories);
        var result = await task;
        return Result.Ok(result.Value.Select(a => a.ToAttributeCategoryDto()).ToList());
    }
}