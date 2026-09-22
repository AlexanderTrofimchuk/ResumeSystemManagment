namespace ResumeSystemManagement.Application.DTOs.Position;

public class PositionDetails
{
    public List<PositionDetail> Positions { get; set; } = new();
    public List<int> SelectIds { get; set; } = new();
    public CreatePositionDto CreatePositionDto { get; set; } = null!;
}