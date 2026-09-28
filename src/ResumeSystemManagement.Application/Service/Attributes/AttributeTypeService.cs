using FluentResults;
using ResumeSystemManagement.Application.DTOs.Attributes;
using ResumeSystemManagement.Application.Interfaces.Attributes;
using ResumeSystemManagement.Application.Mappers;
using ResumeSystemManagement.Core.Interfaces.Repositories.AttributeRepository;

namespace ResumeSystemManagement.Application.Service.Attributes;

public class AttributeTypeService (IAttributeTypeRepository repository):IAttributeTypeService
{
    public async Task<Result<List<AttributeTypeDto>>> GetAttributeTypes()
    {
        var task = Result.Try(repository.GetAttributeTypes);
        var result = await task;
        return Result.Ok(result.Value.Select(a =>a.ToAttributeTypeDto()).ToList());
    }
}