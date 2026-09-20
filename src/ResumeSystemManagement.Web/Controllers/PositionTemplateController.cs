using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResumeSystemManagement.Application.DTOs.Attributes;
using ResumeSystemManagement.Application.Interfaces.Attributes;
using ResumeSystemManagement.Application.Interfaces.Position;
using ResumeSystemManagement.Core.Enums;
using ResumeSystemManagement.Core.ReadModels;
using ResumeSystemManagement.Web.ViewModels.Enums;

namespace ResumeSystemManagement.Web.Controllers;

public class PositionTemplateController(IAttributeTypeService typeService, IAttributeLibraryService libraryService, IPositionTemplateService templateService): BaseController
{
    private readonly IAttributeTypeService _typeService = typeService;
    private readonly IAttributeLibraryService _libraryService = libraryService;
    private readonly IPositionTemplateService _templateService = templateService;
    
    [Authorize(Roles = RoleNames.Recruiter)]
    public async Task<IActionResult> Template(int id)
    { 
        var resultTemplate = await _templateService.GetById(id);
        if (resultTemplate.IsFailed) return RedirectWithMessage(GetErrorsMessage(resultTemplate.ToResult()),
            MessageColor.Danger, ActionName.Index, ControllerName.Position);
        return View("PositionTemplate", resultTemplate.Value);
    }

    [Authorize(Roles = RoleNames.Recruiter)]
    [HttpGet]
    public async Task<IActionResult> GetAttributeLibrary(int page, int pageSize, int positionId)
    {
        var attributes = await _libraryService.GetAttributesAsync(pageSize, page);
        var attributeTemplate = attributes.Value.Attributes.Select(a => a.ToAttributeTemplate());
        var addedResult = await _templateService.GetAttributeInPosition(positionId);
        ViewBag.AddedAttributes = addedResult.Value;
        return PartialView("_LibraryPartial",attributeTemplate);
    }

    [Authorize(Roles = RoleNames.Recruiter)]
    [HttpGet]
    public async Task<IActionResult> GetAttributeTypes()
    {
        var types = await _typeService.GetAttributeTypes();
        return PartialView("_TypePartial", types.Value);
    }

    [Authorize(Roles = RoleNames.Recruiter)]
    [HttpPost]
    public async Task<IActionResult> AddAttribute(int positionId, int attributeId, TemplateSection section)
    {
        var assignResult = await _templateService.AssignAttributeAsync(positionId, attributeId, section);
        if (assignResult.IsFailed) return ReturnCurrentException(
            GetErrorsMessage(assignResult), ActionName.Template);
        return RedirectWithMessage(["Attribute is assigned"],
            MessageColor.Success, ActionName.Template,ControllerName.PositionTemplate, new {Id = positionId});
    }

    [Authorize(Roles = RoleNames.Recruiter)]
    [HttpPost]
    public async Task<IActionResult> MoveAttribute(int positionId, int attributeId, TemplateSection section)
    {
        var updateResult = await _templateService.UpdateSectionAsync(positionId, attributeId, section);
        if (updateResult.IsFailed)  return ReturnCurrentException(
            GetErrorsMessage(updateResult), ActionName.Template);
        return RedirectWithMessage(["Attribute is moved"],
            MessageColor.Success, ActionName.Template,ControllerName.PositionTemplate, new {Id = positionId});
    }

    [Authorize(Roles = RoleNames.Recruiter)]
    [HttpPost]
    public async Task<IActionResult> RemoveAttribute(int positionId, int attributeId)
    {
        var deleteResult = await _templateService.DeleteAttributeAsync(positionId, attributeId);
        if (deleteResult.IsFailed)  return ReturnCurrentException(
            GetErrorsMessage(deleteResult), ActionName.Template);
        return RedirectWithMessage(["Attribute is deleted from template"],
            MessageColor.Success, ActionName.Template,ControllerName.PositionTemplate, new {Id = positionId});
    }
    
    private async Task PopulateDate(int page, int pageSize)
    {
        await PopulateTypeList();
    }
    private async Task PopulateTypeList()
    {
        
    }
}