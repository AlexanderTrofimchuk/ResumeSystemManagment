namespace ResumeSystemManagement.Application.DTOs.Attributes;

public record EditAttributeDto(
    int Id,
    int TypeId,
    int CategoryId,
    string Title,
    string Description,
    bool IsBuiltIn,
    List<DropdownOption>? DropdownOptions = null);
