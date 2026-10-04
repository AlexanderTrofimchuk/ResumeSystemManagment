using Microsoft.AspNetCore.Mvc;
using ResumeSystemManagement.Application.DTOs.Ticket;
using ResumeSystemManagement.Application.Interfaces.Ticket;
using ResumeSystemManagement.Web.ViewModels.Enums;

namespace ResumeSystemManagement.Web.Controllers;

public class SupportTicketController(ISupportTicketService supportTicketService) : BaseController
{
    private readonly ISupportTicketService _supportTicketService = supportTicketService;

    [HttpPost]
    public async Task<IActionResult> CreateTicket(SupportTicket supportTicket)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _supportTicketService.CreateTicketAsync(supportTicket);
        if (result.IsFailed) return RedirectWithMessage(GetErrorsMessage(result),
            MessageColor.Danger,ActionName.Index, ControllerName.Home);
        TempData["ToastMessages"] = "Support ticket submitted successfully.";
        TempData["ToastType"] = nameof(MessageColor.Success).ToLower();
        return Redirect(supportTicket.Link);
    }
}