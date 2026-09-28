using ResumeSystemManagement.Core.ReadModels;

namespace ResumeSystemManagement.Application.DTOs.Positions;

public class PositionDetails
{
    public List<PositionDetail> Positions { get; set; } = new();
    public List<int> SelectIds { get; set; } = new();
    public CreatePositionDto CreatePositionDto { get; set; } = null!;
    public int CurrentPage { get; set; }
    public int PageSize { get; set; } = PaginationConstants.DefaultPageSize;
    public int TotalCount { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
}