namespace ResumeSystemManagement.Web.ViewModels;

public class LatestPositionViewModel
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string? CompanyName { get; set; }

    public string? Level { get; set; }

    public string? Location { get; set; }

    public DateTime UpdatedAt { get; set; }
}