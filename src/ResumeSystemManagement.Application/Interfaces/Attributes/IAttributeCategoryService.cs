using FluentResults;
using ResumeSystemManagement.Application.DTOs.Attribute;

namespace ResumeSystemManagement.Application.Interfaces.Attributes;

public interface IAttributeCategoryService
{
    Task<Result<List<AttributeCategoryDTo>>> GetAttributeTypes();
}