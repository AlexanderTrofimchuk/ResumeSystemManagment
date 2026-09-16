using FluentResults;
using ResumeSystemManagement.Application.DTOs.Attributes;

namespace ResumeSystemManagement.Application.Interfaces.Attributes;

public interface IAttributeTypeService
{
    Task<Result<List<AttributeTypeDTo>>> GetAttributeTypes();
}