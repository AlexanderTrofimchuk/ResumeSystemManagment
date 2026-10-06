using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResumeSystemManagement.Application.DTOs.Attributes;
using ResumeSystemManagement.Application.Interfaces.Auth;
using ResumeSystemManagement.Application.Interfaces.SalesForce;
using ResumeSystemManagement.Application.Interfaces.Users;

namespace ResumeSystemManagement.Web.Controllers;

public class ProfileController(
    IProfileService profileService,
    IUserContext userContext,
    ISalesForceService salesForceService) : BaseController
{
    private readonly IProfileService _profileService = profileService;
    private readonly IUserContext _userContext = userContext;
    private readonly ISalesForceService _salesForceService = salesForceService;

    [Authorize]
    public async Task<IActionResult> Profile()
    {
        var profileDetails = await _profileService.GetProfileDetails(_userContext.UserId.ToString());
        return View(profileDetails);
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAttributeLibrary(int page = 1)
    {
        var attributes = await _profileService.GetAttributeTemplate(page);
        return PartialView("_AddAttribute",attributes);
    }    

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAttributeField(int id)
    {
        var attribute = await _profileService.GetAttributeValueTemplate(id, _userContext.UserId.ToString());
        if (attribute is null)
            return NotFound();

        return PartialView("_AttributeField", attribute);
    }
    
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> MeSectorSave([FromBody] List<UserAttributeValue>? sectorValues)
    {
        if (sectorValues is null)
            return BadRequest(new { message = "Request body must contain profile attribute values." });
        var resultUpdate = await _profileService.UpdateMeSector(sectorValues);
        if (resultUpdate.IsFailed)
            return BadRequest(new 
                { message = "Failed to update sector values.", 
                    errors = resultUpdate.Errors.Select(e => e.Message) });
        return Ok(new { received = sectorValues.Count });
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> CreateForm()
    {
        var model = await _salesForceService.GetInfoForForm(_userContext.UserId.ToString());
        return PartialView("_CreateSalesForceForm", model);
    }
}