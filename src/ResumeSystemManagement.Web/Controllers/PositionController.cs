using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResumeSystemManagement.Application.DTOs.Position;
using ResumeSystemManagement.Application.Interfaces.Position;
using ResumeSystemManagement.Core.ReadModels;
using ResumeSystemManagement.Web.ViewModels.Enums;

namespace ResumeSystemManagement.Web.Controllers;

public class PositionController(IPositionService positionService): BaseController
{
    private readonly IPositionService _positionService = positionService;
    public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
    {
        var positons = await _positionService.GetAllPositions(page, pageSize);
        return View(positons.Value);
    }

    [HttpGet]
    public async Task<IActionResult> GetPositionDetail(int positionId)
    {
        var positionResult = await _positionService.GetPositionDetailForUser(positionId);
        if (positionResult.IsFailed) return AlertErrors(positionResult.ToResult());
        return View("PositionDetail",positionResult.Value);
    }

    [HttpPost]
    public async Task<IActionResult> SearchPosition(string name)
    {
        var searchResult = await _positionService.GetAllPositionsByName(name);
        if (searchResult.IsFailed)  return AlertErrors(searchResult.ToResult());
        return  View("Index", searchResult.Value);
    }
    
    [Authorize(Roles = $"{RoleNames.Recruiter},{RoleNames.Administrator}")]
    [HttpPost]
    public async Task<IActionResult> CreatePosition(CreatePositionDto model)
    {
        var resultCreated = await _positionService.CreatePosition(model);
        if (resultCreated.IsFailed) return AlertErrors(resultCreated.ToResult());
        return TemplateRedirect(new {Id = resultCreated.Value});
    }

    private RedirectToActionResult TemplateRedirect(object routeValues)
    {
        return RedirectToAction(nameof(ActionName.Template),nameof(ControllerName.PositionTemplate),
            routeValues);
    }

    [Authorize(Roles = $"{RoleNames.Recruiter},{RoleNames.Administrator}")]
    [HttpGet]
    public async Task<IActionResult> EditPositionForm(int id)
    {
        var positionResult = await _positionService.GetEditPosition(id);
        if (positionResult.IsFailed) return AlertErrors(positionResult.ToResult());
        return PartialView("_EditPosition", positionResult.Value);
    }
    
    [Authorize(Roles = $"{RoleNames.Recruiter},{RoleNames.Administrator}")]
    [HttpPost]
    public async Task<IActionResult> DuplicatePosition(PositionDetails model)
    {
        var resultDuplicate = await _positionService.DuplicatePosition(model.SelectIds[0]);
        if (resultDuplicate.IsFailed) return AlertErrors(resultDuplicate);
        return AlertSuccessOperation("Position was duplicate successfully.");
    }
    
    [Authorize(Roles = $"{RoleNames.Recruiter},{RoleNames.Administrator}")]
    [HttpPost]
    public async Task<IActionResult> EditPosition(EditPositionDto model)
    {
        var editResult = await _positionService.EditPosition(model);
        if (editResult.IsFailed) return AlertErrors(editResult);
        return AlertSuccessOperation("Position was edit successfully.");
    }

    [Authorize(Roles = $"{RoleNames.Recruiter},{RoleNames.Administrator}")]
    [HttpPost]
    public IActionResult RenderTemplate(PositionDetails model) =>
        TemplateRedirect(new {Id = model.SelectIds[0]});
    
    [Authorize(Roles = $"{RoleNames.Recruiter},{RoleNames.Administrator}")]
    [HttpPost]
    public async Task<IActionResult> DeletePositions(PositionDetails model)
    {
        var resultDelete = await _positionService.DeletePosition(model.SelectIds);
        if (resultDelete.IsFailed) return AlertErrors(resultDelete);
        return AlertSuccessOperation("Position/positions was deleted successfully.");
    }

    private IActionResult AlertSuccessOperation(string message)
    {
        return RedirectWithMessage([message],
            MessageColor.Success,ActionName.Index,ControllerName.Position);
    }

    private IActionResult AlertErrors(Result result)
    {
        return ReturnCurrentException(GetErrorsMessage(result), ActionName.Index);
    }
}