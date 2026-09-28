using ResumeSystemManagement.Core.Enums;

namespace ResumeSystemManagement.Application.DTOs.Positions;

public class LatestPosition
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public PositionLevel? Level { get; set; }

    public DateTime Created { get; set; }
}