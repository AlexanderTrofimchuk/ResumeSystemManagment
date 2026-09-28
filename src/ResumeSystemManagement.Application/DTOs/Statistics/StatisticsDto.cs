using ResumeSystemManagement.Application.DTOs.Positions;
using ResumeSystemManagement.Application.DTOs.Tags;

namespace ResumeSystemManagement.Application.DTOs.Statistics;

public class StatisticsDto
{
    public int TotalResumes { get; set; }
    public int TotalPositions { get; set; }
    public int TotalCandidates { get; set; }
    public int ResumesLast24Hours { get; set; }

    // Latest Positions
    public List<LatestPosition> LatestPositions { get; set; }
        = new();

    // Most Popular Positions
    public List<PopularPosition> MostPopularPositions { get; set; }
        = new();

    // Technology Tags
    public List<TechnologyTag> TechnologyTags { get; set; }
        = new();
}