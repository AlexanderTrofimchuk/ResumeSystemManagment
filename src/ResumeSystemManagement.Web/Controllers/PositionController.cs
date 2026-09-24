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

    [HttpGet]
    public async Task<IActionResult> GetPositionDetail(int positionId)
    {
        var positionResult = await service.GetPositionDetailForUser(positionId);
        if (positionResult.IsFailed) return ReturnCurrentException(
            GetErrorsMessage(positionResult.ToResult()), ActionName.Index);
        return View("PositionDetail",positionResult.Value);
    }

    [HttpPost]
    public async Task<IActionResult> SearchPosition(string name)
    {
        var searchResult = await service.GetAllPositionsByName(name);
        if (searchResult.IsFailed)  return ReturnCurrentException(
            GetErrorsMessage(searchResult.ToResult()), ActionName.Index);
        return  View("Index", searchResult.Value);
    }
    
    [Authorize(Roles = $"{RoleNames.Recruiter},{RoleNames.Administrator}")]
    [HttpPost]
    public async Task<IActionResult> CreatePosition(CreatePositionDto model)
    {
        var resultCreated = await service.CreatePosition(model);
        if (resultCreated.IsFailed) return ReturnCurrentException(
                GetErrorsMessage(resultCreated.ToResult()), ActionName.Index);
        return RedirectWithMessage(["Position was created successfully."],
            MessageColor.Success,ActionName.Template,ControllerName.PositionTemplate,new {Id = resultCreated.Value});
    }

    [Authorize(Roles = $"{RoleNames.Recruiter},{RoleNames.Administrator}")]
    [HttpGet]
    public async Task<IActionResult> EditPositionForm(int id)
    {
        var positionResult = await service.GetEditPosition(id);
        if (positionResult.IsFailed) return ReturnCurrentException(
            GetErrorsMessage(positionResult.ToResult()), ActionName.Index);
        return PartialView("_EditPosition", positionResult.Value);
    }
    
    [Authorize(Roles = $"{RoleNames.Recruiter},{RoleNames.Administrator}")]
    [HttpPost]
    public async Task<IActionResult> DuplicatePosition(PositionDetails model)
    {
        var resultDuplicate = await service.DuplicatePosition(model.SelectIds[0]);
        if (resultDuplicate.IsFailed) return ReturnCurrentException(
            GetErrorsMessage(resultDuplicate), ActionName.Index);
        return RedirectWithMessage(["Position was duplicate successfully."],
            MessageColor.Success,ActionName.Index,ControllerName.Position);
    }
    
    [Authorize(Roles = $"{RoleNames.Recruiter},{RoleNames.Administrator}")]
    [HttpPost]
    public async Task<IActionResult> EditPosition(EditPositionDto model)
    {
        var editResult = await service.EditPosition(model);
        if (editResult.IsFailed) return ReturnCurrentException(
            GetErrorsMessage(editResult), ActionName.Index);
        return RedirectWithMessage(["Position was edit successfully."],
            MessageColor.Success,ActionName.Index,ControllerName.Position);
    }

    [Authorize(Roles = $"{RoleNames.Recruiter},{RoleNames.Administrator}")]
    [HttpPost]
    public IActionResult RenderTemplate(PositionDetails model) =>
        RedirectToAction(nameof(ActionName.Template),  nameof(ControllerName.PositionTemplate),
            routeValues:new {Id = model.SelectIds[0]});
    
    [Authorize(Roles = $"{RoleNames.Recruiter},{RoleNames.Administrator}")]
    [HttpPost]
    public async Task<IActionResult> DeletePositions(PositionDetails model)
    {
        var resultDelete = await service.DeletePosition(model.SelectIds);
        if (resultDelete.IsFailed) return ReturnCurrentException(
            GetErrorsMessage(resultDelete), ActionName.Index);
        return RedirectWithMessage(["Position/positions was deleted successfully."],
            MessageColor.Success,ActionName.Index,ControllerName.Position);
    }
}