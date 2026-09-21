using FluentResults;
using ResumeSystemManagement.Application.DTOs.Attribute;

namespace ResumeSystemManagement.Application.Interfaces.Attributes;

public interface IAttributeTypeService
{
    Task<Result<List<AttributeTypeDTo>>> GetAttributeTypes();
}