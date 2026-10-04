using ResumeSystemManagement.Core.Enums;

namespace ResumeSystemManagement.Infrastructure.Serialization.JsonEntity;

public class SupportTicketJson
{
    public string ReportedBy { get; set; } = null!;
    public string? PositionName { get; set; }
    public string Summary { get; set; } = null!;
    public TicketPriority Priority { get; set; }
    public HashSet<string> AdminEmails { get; set; } = new();
    public string? Link { get; set; }
}