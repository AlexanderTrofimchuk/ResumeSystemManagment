using FluentResults;
using ResumeSystemManagement.Application.DTOs.Attribute;
using ResumeSystemManagement.Application.DTOs.Attribute.Mapper;
using ResumeSystemManagement.Application.Interfaces.Attributes;
using ResumeSystemManagement.Core.Interfaces.Repositories.AttributeRepository;

namespace ResumeSystemManagement.Application.Service.Attributes;

public class AttributeTypeService (IAttributeTypeRepository repository):IAttributeTypeService
{
    public async Task<Result<List<AttributeTypeDTo>>> GetAttributeTypes()
    {
        var task = Result.Try(repository.GetAttributeTypes);
        var result = await task;
        return Result.Ok(result.Value.Select(a =>a.ToAttributeTypeDTo()).ToList());
    }
}