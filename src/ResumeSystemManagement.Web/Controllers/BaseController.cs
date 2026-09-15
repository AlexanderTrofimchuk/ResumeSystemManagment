using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using ResumeSystemManagement.Web.ViewModels.Enums;

namespace ResumeSystemManagement.Web.Controllers;

public class BaseController: Controller
{
    public IActionResult RedirectWithMessage(List<string> messages, MessageColor color, ActionName action, 
        ControllerName controller, object? router = null)
    {
        TempData["ToastMessages"] = JsonSerializer.Serialize(messages);
        TempData["ToastType"] = color.ToString().ToLower();
        return RedirectToAction(action.ToString(), controller.ToString(),  router);
    }

    public IActionResult ReturnCurrentException(List<string> messages, ActionName action, object model)
    {
        TempData["ToastMessages"] = JsonSerializer.Serialize(messages);
        TempData["ToastType"] = nameof(MessageColor.Danger).ToLower();
        return View(action.ToString(), model);
    }
}