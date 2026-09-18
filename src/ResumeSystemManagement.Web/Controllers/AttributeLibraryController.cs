using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ResumeSystemManagement.Application.DTOs.Attributes;
using ResumeSystemManagement.Application.Interfaces.Attributes;
using ResumeSystemManagement.Core.ReadModels;
using ResumeSystemManagement.Web.ViewModels.Enums;

namespace ResumeSystemManagement.Web.Controllers;

[Authorize(Roles = RoleNames.Recruiter)]
public class AttributeLibraryController(
    IAttributeCategoryService categoryService,
    IAttributeTypeService typeService,
    IAttributeLibraryService libraryService)
    : BaseController
{
    private readonly IAttributeLibraryService _libraryService = libraryService;
    private readonly IAttributeTypeService _typeService = typeService;
    private readonly IAttributeCategoryService _categoryService = categoryService;

    [Authorize(Roles = RoleNames.Recruiter)]
    public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
    {
        var attributes = await _libraryService.GetAttributesAsync(pageSize, page);
        if (attributes.IsFailed)
            return ReturnCurrentException(attributes.Errors.Select(e => e.Message).ToList(),
                ActionName.Index);
        await PopulateDropdowns();
        return View(attributes.Value);
    }

    [Authorize(Roles = RoleNames.Recruiter)]
    [HttpGet]
    public async Task<IActionResult> SearchAttribute(string name)
    {
        var attributes = await _libraryService.GetAttributesByNameAsync(name);
        if (attributes.IsFailed)
            return ReturnCurrentException(attributes.Errors.Select(e => e.Message).ToList(),
                ActionName.Index);
        return View("Index", attributes.Value);
    }

    [Authorize(Roles = RoleNames.Recruiter)]
    [HttpPost]
    public async Task<IActionResult> CreateAttribute(CreateAttributeDto dto)
    {
        var idCreated = await _libraryService.CreateAttributeAsync(dto);
        if (idCreated.IsFailed) return ReturnCurrentException(
            idCreated.Errors.Select(e => e.Message).ToList(), ActionName.Index, dto);
        await AddDropDownOptions(dto, idCreated.Value);
        return RedirectWithMessage(["Attributes created"], 
            MessageColor.Success,ActionName.Index, ControllerName.AttributeLibrary);
    }

    [Authorize(Roles = RoleNames.Recruiter)]
    [HttpGet]
    public async Task<IActionResult> EditAttributeForm(int id)
    {
        var attribute = await _libraryService.GetEditAttributeAsync(id);
        if (attribute.IsFailed)
            return ReturnCurrentException(attribute.Errors.Select(e => e.Message).ToList(),
                ActionName.Index);
        await PopulateDropdowns();
        return PartialView("_EditAttribute", attribute.Value);
    }

    [Authorize(Roles = RoleNames.Recruiter)]
    [HttpPost]
    public async Task<IActionResult> EditAttribute(EditAttributeDTo dto)
    {
        var result = await _libraryService.EditAttributeAsync(dto);
        if (result.IsFailed)
            return ReturnCurrentException(result.Errors.Select(e => e.Message).ToList(),
                ActionName.Index);
        return RedirectWithMessage(["Attribute updated"],
            MessageColor.Success, ActionName.Index, ControllerName.AttributeLibrary);
    }

    [Authorize(Roles = RoleNames.Recruiter)]
    [HttpPost]
    public async Task<IActionResult> DeleteAttributes(AttributeDetails model)
    {
        if (model.SelectIds.Count == 0)
            return ReturnCurrentException(["Select attributes"],
                ActionName.Index);

        var result = await _libraryService.BulkDeleteAttributesAsync(model.SelectIds);
        if (result.IsFailed)
            return ReturnCurrentException(result.Errors.Select(e => e.Message).ToList(),
                ActionName.Index);
        return RedirectWithMessage(["Attributes deleted"],
            MessageColor.Success, ActionName.Index, ControllerName.AttributeLibrary);
    }

    private async Task AddDropDownOptions(CreateAttributeDto dto, int id)
    {
        if (dto.DropDownOptions != null && dto.DropDownOptions.Count != 0) {
            var isAdded = await _libraryService.AddDropDownOptions(id, dto);
            if (isAdded.IsFailed)
            {
                ReturnCurrentException(
                    isAdded.Errors.Select(e => e.Message).ToList(), ActionName.Index, dto);
            }
        }
    }
    
    private async Task PopulateDropdowns()
    {
        await PopulateDropdownsCategory();
        await PopulateDropdownsType();
    }

    private async Task PopulateDropdownsCategory()
    {
        var categories = await _categoryService.GetAttributeTypes();
        ViewBag.Categories = categories.Value
            .Select(c => new SelectListItem(c.Title, c.Id.ToString()))
            .ToList();
    }

    private async Task PopulateDropdownsType()
    {
        var types = await _typeService.GetAttributeTypes();
        ViewBag.Types = types.Value
            .Select(t => new SelectListItem(t.Title, t.Id.ToString()))
            .ToList();
        ViewBag.DropdownTypeId = types.Value
            .Where(a => a.Title == "One of many").Select(t => t.Id).FirstOrDefault();
    }

}