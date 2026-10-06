using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResumeSystemManagement.Application.DTOs.SalesForce;
using ResumeSystemManagement.Application.Interfaces.SalesForce;
using ResumeSystemManagement.Web.ViewModels.Enums;

namespace ResumeSystemManagement.Web.Controllers;

public class SalesForceController(ISalesForceService salesForceService) : BaseController
{
    private readonly ISalesForceService _salesForceService = salesForceService;

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateAccount(CreateAccountSalesForce model)
    {
        if (!ModelState.IsValid)
            return RedirectWithMessage(["Please complete all required fields correctly."],
                MessageColor.Danger, ActionName.Index, ControllerName.Profile);

        if (await _salesForceService.HasAccountAndContact())
            return RedirectWithMessage(["Salesforce account and contact already exist."],
                MessageColor.Danger, ActionName.Profile, ControllerName.Profile);
        
        var result = await _salesForceService.CreateAccountAndContact(model);
        if (result.IsFailed) return RedirectWithMessage(GetErrorsMessage(result),
                MessageColor.Danger, ActionName.Profile, ControllerName.Profile);
        
        return RedirectWithMessage(["Salesforce account and contact created successfully."],
            MessageColor.Success, ActionName.Profile, ControllerName.Profile);
    }
}