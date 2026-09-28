namespace ResumeSystemManagement.Web.ViewModels;

public class PaginationViewModel
{
    public int CurrentPage { get; init; }
    public int TotalPages { get; init; }
    public int TotalCount { get; init; }
    public string Controller { get; init; } = string.Empty;
    public string Action { get; init; } = string.Empty;
    public string PageParameterName { get; init; } = "page";
    public Dictionary<string, string> RouteValues { get; init; } = new();
}
