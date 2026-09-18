using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResumeSystemManagement.Application.DTOs.Position;
using ResumeSystemManagement.Application.Interfaces.Position;
using ResumeSystemManagement.Core.ReadModels;
using ResumeSystemManagement.Web.ViewModels.Enums;

namespace ResumeSystemManagement.Web.Controllers;

public class PositionController(IPositionService service): BaseController
{
    public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
    {
        var positons = await service.GetAllPositions(page, pageSize);
        return View(positons.Value);
    }

    [Authorize(Roles = RoleNames.Recruiter)]
    [HttpPost]
    public async Task<IActionResult> CreatePosition(CreatePositionDto model)
    {
        var resultCreated = await service.CreatePosition(model);
        if (resultCreated.IsFailed) return ReturnCurrentException(
                resultCreated.Errors.Select(e => e.Message).ToList(), ActionName.Index);
        return RedirectWithMessage(["Position was created successfully."],
            MessageColor.Success,ActionName.Index,ControllerName.Position);
    }

    [Authorize(Roles = RoleNames.Recruiter)]
    [HttpGet]
    public async Task<IActionResult> EditPositionForm(int id)
    {
        var position = await service.GetEditPosition(id);
        if (position.IsFailed) return ReturnCurrentException(
            position.Errors.Select(e => e.Message).ToList(), ActionName.Index);
        return PartialView("_EditPosition", position.Value);
    }
    
    [Authorize(Roles = RoleNames.Recruiter)]
    [HttpPost]
    public async Task<IActionResult> DuplicatePosition(PositionDetails model)
    {
        var resultDuplicate = await service.DuplicatePosition(model.SelectIds[0]);
        if (resultDuplicate.IsFailed) return ReturnCurrentException(
            resultDuplicate.Errors.Select(e => e.Message).ToList(), ActionName.Index);
        return RedirectWithMessage(["Position was duplicate successfully."],
            MessageColor.Success,ActionName.Index,ControllerName.Position);
    }

    [Authorize(Roles = RoleNames.Recruiter)]
    [HttpPost]
    public async Task<IActionResult> EditPosition(EditPositionDto model)
    {
        var editResult = await service.EditPosition(model);
        if (editResult.IsFailed) return ReturnCurrentException(
            editResult.Errors.Select(e => e.Message).ToList(), ActionName.Index);
        return RedirectWithMessage(["Position was edit successfully."],
            MessageColor.Success,ActionName.Index,ControllerName.Position);
    }
    
    [Authorize(Roles = RoleNames.Recruiter)]
    [HttpPost]
    public async Task<IActionResult> DeletePositions(PositionDetails model)
    {
        var resultDelete = await service.DeletePosition(model.SelectIds);
        if (resultDelete.IsFailed) return ReturnCurrentException(
            resultDelete.Errors.Select(e => e.Message).ToList(), ActionName.Index);
        return RedirectWithMessage(["Position/positions was deleted successfully."],
            MessageColor.Success,ActionName.Index,ControllerName.Position);
    }
}