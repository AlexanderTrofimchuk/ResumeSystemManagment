namespace ResumeSystemManagement.Core.JsonEntity;

public class SupportTicketJson
{
    public string ReportedBy { get; set; } = null!;
    public string? PositionName { get; set; }
    public string Summary { get; set; } = null!;
    public string Priority { get; set; } = null!;
    public HashSet<string> AdminEmails { get; set; } = new();
    public string? Link { get; set; }
}