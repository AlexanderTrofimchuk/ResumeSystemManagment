namespace ResumeSystemManagement.Application.DTOs.Position;

public class PositionDetails
{
    public List<PositionDetail> Positions { get; set; } 
    public List<int> SelectIds { get; set; }
    public CreatePositionDto CreatePositionDto { get; set; }
}