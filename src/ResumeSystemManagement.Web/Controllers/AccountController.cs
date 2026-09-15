using Microsoft.AspNetCore.Mvc;
using ResumeSystemManagement.Core.Interfaces;
using ResumeSystemManagement.Web.ViewModels;
using ResumeSystemManagement.Web.ViewModels.Enums;

namespace ResumeSystemManagement.Web.Controllers;

public class AccountController(ILoginService loginService, IRegistrationService registrationService): BaseController
{
    public IActionResult LoginPage()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> LoginUser(LoginViewModel model)
    {
        if (!ModelState.IsValid) return ReturnCurrentException(
            ["Введены не правельные данные"], ActionName.LoginPage, 
            new LoginViewModel {Email = model.Email});
        var result = await loginService.Login(model.Email, model.Password);
        return result.IsFailed ? 
            ReturnCurrentException(result.Errors.Select(e => e.Message).ToList(), 
                ActionName.LoginPage, 
                new LoginViewModel {Email = model.Email}) 
            : RedirectWithMessage(["Вы вошли"], MessageColor.Success, ActionName.Index, ControllerName.Home);
    }

    public IActionResult RegisterPage()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> RegisterUser(RegistrationViewModel model)
    {
        if (!ModelState.IsValid) return ReturnCurrentException(["Вы не вели данные"],ActionName.RegisterPage, 
            new RegistrationViewModel {FullName = model.FullName, Email = model.Email});
        var result = await registrationService.Register(model.FullName, model.Email, model.Password);
        if (result.IsFailed)
            return ReturnCurrentException(result.Errors.Select(e => e.Message).ToList(), 
                ActionName.RegisterPage,
                new RegistrationViewModel {FullName = model.FullName, Email = model.Email});
        return RedirectWithMessage(["Вы зарегистрировались"], MessageColor.Success, ActionName.Index, ControllerName.Home);
    }

    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await loginService.Logout();
        return RedirectToAction(nameof(ActionName.Index), nameof(ControllerName.Home));
    }
}