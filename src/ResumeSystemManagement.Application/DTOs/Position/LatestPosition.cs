using ResumeSystemManagement.Core.Enums;

namespace ResumeSystemManagement.Application.DTOs.Statistic;

public class LatestPosition
{
    public int Id { get; set; }

    public string Title { get; set; }

    public PositionLevel? Level { get; set; }

    public DateTime Created { get; set; }
}