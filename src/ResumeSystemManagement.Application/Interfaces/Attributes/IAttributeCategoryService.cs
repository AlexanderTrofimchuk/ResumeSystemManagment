using FluentResults;
using ResumeSystemManagement.Application.DTOs.Attributes;

namespace ResumeSystemManagement.Application.Interfaces.Attributes;

public interface IAttributeCategoryService
{
    Task<Result<List<AttributeCategoryDTo>>> GetAttributeTypes();
}