namespace ResumeSystemManagement.Application.DTOs.Attributes;

public record AttributeTemplate(int Id, string Name, string? Description = null, string? TypeName =  null);
