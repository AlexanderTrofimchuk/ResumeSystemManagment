namespace ResumeSystemManagement.Web.ViewModels;

public class HomeViewModel
{
    // Statistics
    public int TotalCvs { get; set; } = 10;
    public int TotalPositions { get; set; } = 10;
    public int TotalCandidates { get; set; } = 10;
    public int CvsLast24Hours { get; set; } = 10;

    // Latest Positions
    public List<LatestPositionViewModel> LatestPositions { get; set; }
        = [];

    // Most Popular Positions
    public List<PopularPositionViewModel> MostPopularPositions { get; set; }
        = [];

    // Technology Tags
    public List<TechnologyTagViewModel> TechnologyTags { get; set; }
        = [];
}