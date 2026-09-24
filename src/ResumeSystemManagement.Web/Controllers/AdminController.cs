using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResumeSystemManagement.Application.DTOs.User;
using ResumeSystemManagement.Application.Interfaces.User;
using ResumeSystemManagement.Core.ReadModels;
using ResumeSystemManagement.Web.ViewModels.Enums;

namespace ResumeSystemManagement.Web.Controllers;

public class AdminController(IUserService userService): BaseController
{
    [Authorize(Roles = RoleNames.Administrator)]
    public async Task<IActionResult> UserManagement(int pageNumber= 1, int pageSize = 50)
    {
        var users = await userService.GetAllRecordAsync(pageNumber, pageSize);
        return View(users);
    }

    [Authorize(Roles = RoleNames.Administrator)]
    [HttpPost]
    public async Task<IActionResult> ChangeRole(UserDetails model, string role)
    {
        if(!model.SelectIds.Any()) return ReturnCurrentException(
            ["You don't select any record."], ActionName.UserManagement);
        var changeResult = await userService.ChangeRoleAsync(model.SelectIds, role);
        if (changeResult.IsFailed) return RedirectWithMessage(
            GetErrorsMessage(changeResult), MessageColor.Danger,ActionName.UserManagement
            , ControllerName.Admin,new {pageNumber = model.CurrentPage, pageSize = model.PageSize});
        return RedirectWithMessage(["Users change role"],
            MessageColor.Success,ActionName.UserManagement,ControllerName.Admin);
    }
    
    [Authorize(Roles = RoleNames.Administrator)]
    [HttpPost]
    public async Task<IActionResult> BlockUsers(UserDetails model)
    {
        return await OperationUser(model, "Users were blocked", userService.BlockUserAsync);
    }
    
    [Authorize(Roles = RoleNames.Administrator)]
    [HttpPost]
    public async Task<IActionResult> UnblockUsers(UserDetails model)
    {
        return await OperationUser(model, "Users were unblocked", userService.UnblockUserAsync);
    }
    
    [Authorize(Roles = RoleNames.Administrator)]
    [HttpPost]
    public async Task<IActionResult> DeleteUsers(UserDetails model)
    {
        return await OperationUser(model, "Users were deleted", userService.DeleteUsersAsync );
    }

    private async Task<IActionResult> OperationUser(UserDetails model, string massage, Func<List<string>,Task<Result>> operation)
    {
        if(!model.SelectIds.Any()) return ReturnCurrentException(
            ["You don't select any record."], ActionName.UserManagement,model);
        var result = await operation(model.SelectIds);
        if (result.IsFailed)return RedirectWithMessage(GetErrorsMessage(result),
            MessageColor.Danger,ActionName.UserManagement,ControllerName.Admin, 
            new {PageNumber = model.CurrentPage, PageSize = model.PageSize});
        return RedirectWithMessage([massage],
            MessageColor.Success,ActionName.UserManagement,ControllerName.Admin, new {PageNumber = model.CurrentPage, PageSize = model.PageSize});
    }
}