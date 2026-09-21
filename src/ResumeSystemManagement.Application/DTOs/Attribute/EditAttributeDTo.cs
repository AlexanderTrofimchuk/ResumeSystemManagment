namespace ResumeSystemManagement.Application.DTOs.Attribute;

public record EditAttributeDTo(
    int Id,
    int TypeId,
    int CategoryId,
    string Title,
    string Description,
    bool IsBuiltIn,
    List<DropDownOption>? DropdownOptions = null);