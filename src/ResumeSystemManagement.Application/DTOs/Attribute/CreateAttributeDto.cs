namespace ResumeSystemManagement.Application.DTOs.Attribute;

public record CreateAttributeDto(
    int TypeId,
    int CategoryId,
    string Title,
    string Description,
    bool IsBuiltIn,
    List<string>? DropDownOptions = null);