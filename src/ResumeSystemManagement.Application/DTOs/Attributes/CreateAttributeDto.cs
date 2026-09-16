namespace ResumeSystemManagement.Application.DTOs.Attributes;

public record CreateAttributeDto(
    int TypeId,
    int CategoryId,
    string Title,
    string Description,
    bool IsBuiltIn
    );