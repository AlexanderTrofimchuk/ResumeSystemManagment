namespace ResumeSystemManagement.Application.DTOs.Statistic;

public class StaticticeDto
{
    public int TotalCvs { get; set; } = 10;
    public int TotalPositions { get; set; } = 10;
    public int TotalCandidates { get; set; } = 10;
    public int CvsLast24Hours { get; set; } = 10;

    // Latest Positions
    public List<LatestPosition> LatestPositions { get; set; }
        = [];

    // Most Popular Positions
    public List<PopularPosition> MostPopularPositions { get; set; }
        = [];

    // Technology Tags
    public List<TechnologyTag> TechnologyTags { get; set; }
        = [];
}