using System.Security.Claims;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using ResumeSystemManagement.Application.Interfaces;
using ResumeSystemManagement.Infrastructure.Interfaces;
using ResumeSystemManagement.Web.ViewModels.Enums;

namespace ResumeSystemManagement.Web.Controllers;

public class OAuthController(IExternalLoginService loginService, IExternalAuthProvider externalProvider): BaseController
{
    [HttpGet]
    public IActionResult GoogleLogin()
    {
        var properties = externalProvider.ConfigureGoogleLogin(Url.Action("GoogleCallback"));
        return Challenge(properties, "Google");
    }
    
    [HttpGet]
    public async Task<IActionResult> GoogleCallback()
    {
        return await ExternalCallback(
            externalProvider.GetGooglePrincipal,
            loginService.Login,
            "Google");
    }
    
    [HttpGet]
    public IActionResult FacebookLogin()
    {
        var properties = externalProvider.ConfigureFacebookLogin(Url.Action("FacebookCallback"));
        return Challenge(properties, "Facebook");
    }
    
    [HttpGet]
    public async Task<IActionResult> FacebookCallback()
    {
        return await ExternalCallback(
            externalProvider.GetFacebookPrincipal,
            loginService.Login,
            "Facebook");
    }
    
    private async Task<IActionResult> ExternalCallback
        (Func<Task<ClaimsPrincipal?>> principalAction,Func<ClaimsPrincipal,string, Task<Result>> login, string provider)
    {
        var principal = await principalAction();
        if (principal is null)
            return RedirectWithMessage(["Access is denied"], MessageColor.Danger, 
                ActionName.LoginPage, ControllerName.Account);
        var result = await login(principal, provider);
        if (result.IsFailed) 
            return RedirectWithMessage(result.Errors.Select(e => e.Message).ToList()
                , MessageColor.Danger, ActionName.LoginPage, ControllerName.Account);
        return RedirectToAction(nameof(ActionName.Index), nameof(ControllerName.Home));
    }
}