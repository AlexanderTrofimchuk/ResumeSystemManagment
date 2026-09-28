using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResumeSystemManagement.Application.DTOs.User;
using ResumeSystemManagement.Application.Interfaces.Users;
using ResumeSystemManagement.Core.ReadModels;
using ResumeSystemManagement.Web.ViewModels.Enums;

namespace ResumeSystemManagement.Web.Controllers;

public class AdminController(IUserService userService): BaseController
{
    private readonly IUserService _userService = userService;
    
    [Authorize(Roles = RoleNames.Administrator)]
    public async Task<IActionResult> UserManagement(int pageNumber= 1)
    {
        var users = await _userService.GetAllRecordAsync(pageNumber);
        return View(users);
    }

    [Authorize(Roles = RoleNames.Administrator)]
    [HttpPost]
    public async Task<IActionResult> ChangeRole(UserDetails model, string role)
    {
        if(!model.SelectIds.Any()) 
            return OperationAlert(model.CurrentPage,MessageColor.Danger,["You don't select any record."]);
        var changeResult = await _userService.ChangeRoleAsync(model.SelectIds, role);
        if (changeResult.IsFailed)
            return OperationAlert(model.CurrentPage, MessageColor.Danger, GetErrorsMessage(changeResult));
        return OperationAlert(model.CurrentPage,MessageColor.Success,["Users change role"]);
    }
    
    [Authorize(Roles = RoleNames.Administrator)]
    [HttpPost]
    public async Task<IActionResult> BlockUsers(UserDetails model)
    {
        return await OperationUser(model, "Users were blocked", _userService.BlockUserAsync);
    }
    
    [Authorize(Roles = RoleNames.Administrator)]
    [HttpPost]
    public async Task<IActionResult> UnblockUsers(UserDetails model)
    {
        return await OperationUser(model, "Users were unblocked", _userService.UnblockUserAsync);
    }
    
    [Authorize(Roles = RoleNames.Administrator)]
    [HttpPost]
    public async Task<IActionResult> DeleteUsers(UserDetails model)
    {
        return await OperationUser(model, "Users were deleted", _userService.DeleteUsersAsync );
    }

    private async Task<IActionResult> OperationUser(UserDetails model, string massage, Func<List<string>,Task<Result>> operation)
    {
        if(!model.SelectIds.Any()) return ReturnCurrentException(
            ["You don't select any record."], ActionName.UserManagement,model);
        var result = await operation(model.SelectIds);
        if (result.IsFailed)return OperationAlert(model.CurrentPage, MessageColor.Danger,GetErrorsMessage(result));
        return OperationAlert(model.CurrentPage, MessageColor.Success,[massage]);
    }

    private IActionResult OperationAlert(int page, MessageColor color,List<string> message)
    {
        return RedirectWithMessage(message,
            color,ActionName.UserManagement,ControllerName.Admin, 
            new {PageNumber = page});
    }
}