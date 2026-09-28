using ResumeSystemManagement.Core.ReadModels;

namespace ResumeSystemManagement.Application.DTOs.Attributes;

public class AttributeDetails
{
    public List<AttributeDetail> Attributes { get; set; } = new();
    public List<int> SelectIds { get; set; } = new List<int>();
    public CreateAttributeDto CreateAttributeDto { get; set; } = null!;
    public int CurrentPage { get; set; }
    public int PageSize { get; set; } = PaginationConstants.DefaultPageSize;
    public int TotalCount { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
}