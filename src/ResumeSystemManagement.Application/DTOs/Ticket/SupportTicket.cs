using System.ComponentModel.DataAnnotations;
using ResumeSystemManagement.Core.Enums;

namespace ResumeSystemManagement.Application.DTOs.Ticket;

public class SupportTicket
{
    public string Summary { get; set; } = null!;
    public TicketPriority Priority { get; set; } = TicketPriority.Low;
    public string Link { get; set; } = null!;
    public int? PositionId { get; set; } = null;
}