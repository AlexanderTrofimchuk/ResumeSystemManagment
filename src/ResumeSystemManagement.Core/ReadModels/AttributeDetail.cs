namespace ResumeSystemManagement.Core.ReadModels;

public record AttributeDetail(
    int Id,
    string TypeName,
    string CategoryName,
    string Title,
    string Description,
    bool IsBuiltIn
);