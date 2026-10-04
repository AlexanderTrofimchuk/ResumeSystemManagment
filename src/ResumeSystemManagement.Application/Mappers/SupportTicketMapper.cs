using ResumeSystemManagement.Application.DTOs.Ticket;
using ResumeSystemManagement.Infrastructure.Serialization.JsonEntity;

namespace ResumeSystemManagement.Application.Mappers;

public static class SupportTicketMapper
{
    public static SupportTicketJson MapToJson(this SupportTicket ticket, string userId, string? positionName, HashSet<string> adminEmails)
    {
        return new()
        {
            ReportedBy = userId,
            PositionName = positionName,
            Summary = ticket.Summary,
            Priority = ticket.Priority,
            AdminEmails = adminEmails,
            Link = ticket.Link
        };
    }
}