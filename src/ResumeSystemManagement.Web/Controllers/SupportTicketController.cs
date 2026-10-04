using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResumeSystemManagement.Application.DTOs.Ticket;
using ResumeSystemManagement.Application.Interfaces.Ticket;
using ResumeSystemManagement.Web.ViewModels.Enums;

namespace ResumeSystemManagement.Web.Controllers;

public class SupportTicketController(ISupportTicketService supportTicketService) : BaseController
{
    private readonly ISupportTicketService _supportTicketService = supportTicketService;
    
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateTicket(SupportTicket supportTicket)
    {
        if (!ModelState.IsValid)
            return ResirectByUrl(["Invalid ticket data."], 
                MessageColor.Danger, supportTicket.Link);
        var result = await _supportTicketService.CreateTicketAsync(supportTicket);
        if (result.IsFailed) return RedirectWithMessage(GetErrorsMessage(result),
            MessageColor.Danger,ActionName.Index, ControllerName.Home);
        return ResirectByUrl(["Support ticket submitted successfully."], MessageColor.Success, supportTicket.Link);
    }
}