using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResumeSystemManagement.Application.Interfaces.Attributes;
using ResumeSystemManagement.Application.Interfaces.Positions;
using ResumeSystemManagement.Application.Mappers;
using ResumeSystemManagement.Core.Enums;
using ResumeSystemManagement.Core.ReadModels;
using ResumeSystemManagement.Web.ViewModels.Enums;

namespace ResumeSystemManagement.Web.Controllers;

public class PositionTemplateController(IAttributeTypeService typeService, 
    IAttributeLibraryService libraryService, 
    IPositionTemplateService templateService, 
    IPositionService positionService): BaseController
{
    private readonly IAttributeTypeService _typeService = typeService;
    private readonly IAttributeLibraryService _libraryService = libraryService;
    private readonly IPositionTemplateService _templateService = templateService;
    private readonly IPositionService _positionService = positionService;
    
    [Authorize(Roles = $"{RoleNames.Recruiter},{RoleNames.Administrator}")]
    public async Task<IActionResult> Template(int id)
    { 
        var resultTemplate = await _templateService.GetById(id);
        if (resultTemplate.IsFailed) return RedirectWithMessage(GetErrorsMessage(resultTemplate.ToResult()),
            MessageColor.Danger, ActionName.Index, ControllerName.Position);
        return View("PositionTemplate", resultTemplate.Value);
    }

    [Authorize(Roles = $"{RoleNames.Recruiter},{RoleNames.Administrator}")]
    [HttpGet]
    public async Task<IActionResult> GetAttributeLibrary(int page, int positionId)
    {
        var attributes = await _libraryService.GetAttributesAsync(page);
        var attributeTemplate = attributes.Value.Attributes.Select(a => a.AttributeDetailToTemplate());
        var addedResult = await _templateService.GetAttributeInPosition(positionId);
        ViewBag.AddedAttributes = addedResult.Value;
        return PartialView("_LibraryPartial",attributeTemplate);
    }

    [Authorize(Roles = $"{RoleNames.Recruiter},{RoleNames.Administrator}")]
    [HttpGet]
    public async Task<IActionResult> GetAttributeTypes()
    {
        var types = await _typeService.GetAttributeTypes();
        return PartialView("_TypePartial", types.Value);
    }

    [Authorize(Roles = $"{RoleNames.Recruiter},{RoleNames.Administrator}")]
    [HttpPost]
    public async Task<IActionResult> AddAttribute(int positionId, int attributeId, TemplateSection section)
    {
        var assignResult = await _templateService.AssignAttributeAsync(positionId, attributeId, section);
        return assignResult.IsFailed ? ShowOperationErrors(assignResult) 
            : AlertSuccessExecution(positionId, "Attribute is assigned");
    }

    [Authorize(Roles = $"{RoleNames.Recruiter},{RoleNames.Administrator}")]
    [HttpPost]
    public async Task<IActionResult> MoveAttribute(int positionId, int attributeId, TemplateSection section)
    {
        var updateResult = await _templateService.UpdateSectionAsync(positionId, attributeId, section);
        return updateResult.IsFailed ? ShowOperationErrors(updateResult) 
            : AlertSuccessExecution(positionId, "Attribute is moved");
    }

    [Authorize(Roles = $"{RoleNames.Recruiter},{RoleNames.Administrator}")]
    [HttpPost]
    public async Task<IActionResult> RemoveAttribute(int positionId, int attributeId)
    {
        var deleteResult = await _templateService.DeleteAttributeAsync(positionId, attributeId);
        return deleteResult.IsFailed ? ShowOperationErrors(deleteResult) 
            : AlertSuccessExecution(positionId, "Attribute is deleted from template");
    }

    [Authorize(Roles = $"{RoleNames.Recruiter},{RoleNames.Administrator}")]
    [HttpGet]
    public async Task<IActionResult> PublishPosition(int id)
    {
        var publishResult = await _positionService.PublishPosition(id);
        return publishResult.IsFailed ? ShowOperationErrors(publishResult) 
            : AlertSuccessExecution(id, "Position was published");
    }
    
    [Authorize(Roles = $"{RoleNames.Recruiter},{RoleNames.Administrator}")]
    [HttpGet]
    public async Task<IActionResult> DraftPosition(int id)
    {
        var publishResult = await _positionService.DraftPosition(id);
        return publishResult.IsFailed ? ShowOperationErrors(publishResult) 
            : AlertSuccessExecution(id, "Position was published");
    }

    private IActionResult AlertSuccessExecution(int id, string successMessage)
    {
        return RedirectWithMessage([successMessage],
            MessageColor.Success, ActionName.Template,ControllerName.PositionTemplate, new {Id = id});
    }

    private IActionResult ShowOperationErrors(Result publishResult)
    {
        return ReturnCurrentException(GetErrorsMessage(publishResult), ActionName.Template);
    }
}