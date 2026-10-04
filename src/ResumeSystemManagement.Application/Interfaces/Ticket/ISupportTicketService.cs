using FluentResults;
using ResumeSystemManagement.Application.DTOs.Ticket;

namespace ResumeSystemManagement.Application.Interfaces.Ticket;

public interface ISupportTicketService
{
    Task<Result> CreateTicketAsync(SupportTicket ticket);
}