namespace ResumeSystemManagement.Application.DTOs.Attributes;

public record UserAttributeValue(
    int Id,
    string UserId,
    int AttributeId, 
    string Title,
    string TypeName, 
    string Value,
    List<DropdownOption>? DropdownOptions,
    uint Version);