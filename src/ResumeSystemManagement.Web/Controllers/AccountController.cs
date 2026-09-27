using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResumeSystemManagement.Application.Interfaces.Users;
using ResumeSystemManagement.Core.Interfaces.Service.Login;
using ResumeSystemManagement.Core.Interfaces.Service.Registration;
using ResumeSystemManagement.Core.ReadModels;
using ResumeSystemManagement.Web.ViewModels;
using ResumeSystemManagement.Web.ViewModels.Enums;

namespace ResumeSystemManagement.Web.Controllers;

public class AccountController(ILoginService loginService,IUserService userService, IRegistrationService registrationService): BaseController
{
    private readonly ILoginService _loginService = loginService;
    private readonly IUserService _userService = userService;
    private readonly IRegistrationService _registrationService = registrationService;
    public IActionResult LoginPage() => View();

    [HttpPost]
    public async Task<IActionResult> LoginUser(LoginViewModel model)
    {
        if (!ModelState.IsValid) return ReturnCurrentException(
            ["Invalid data"], ActionName.LoginPage, new LoginViewModel {Email = model.Email});
        var result = await _loginService.Login(model.Email, model.Password);
        return result.IsFailed ? LoginError(model.Email, result) 
            : RedirectToAction(nameof(ActionName.Index), nameof(ControllerName.Home));
    }

    private IActionResult LoginError(string email, Result result)
    {
        return ReturnCurrentException(GetErrorsMessage(result), 
            ActionName.LoginPage, new LoginViewModel {Email = email});
    }

    public IActionResult RegisterPage() => View();

    [HttpPost]
    public async Task<IActionResult> RegisterUser(RegistrationViewModel model)
    {
        if (!ModelState.IsValid) return ReturnCurrentException(["Invalid input"],ActionName.RegisterPage, 
            new RegistrationViewModel {FullName = model.FullName, Email = model.Email});
        var result = await _registrationService.Register(model.FullName, model.Email, model.Password);
        if (result.IsFailed) return RegisterError(model, result);
        return RedirectToAction(nameof(ActionName.Index), nameof(ControllerName.Home));
    }

    private IActionResult RegisterError(RegistrationViewModel model, Result result)
    {
        return ReturnCurrentException(GetErrorsMessage(result), 
            ActionName.RegisterPage,
            new RegistrationViewModel {FullName = model.FullName, Email = model.Email});
    }

    [HttpPost]
    public async Task<IActionResult> ChangePassword(
        [Bind(Prefix = "ChangePassword")] ChangePasswordViewModel model)
    {
        if (!ModelState.IsValid)
            return RedirectWithMessage(["Invalid input"], MessageColor.Danger, 
                ActionName.LoginPage, ControllerName.Account);
        var changeResult = await _userService.ChangePasswordAsync(model.Email, model.OldPassword, model.NewPassword);
        if (changeResult.IsFailed) return LoginError(model.Email, changeResult);
        return RedirectWithMessage(["Password was changed"],
            MessageColor.Success, ActionName.LoginPage, ControllerName.Account);
    }
    
    [Authorize(Roles = $"{RoleNames.Candidate},{RoleNames.Recruiter},{RoleNames.Administrator}")]
    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await _loginService.Logout();
        return RedirectToAction(nameof(ActionName.Index), nameof(ControllerName.Home));
    }
}