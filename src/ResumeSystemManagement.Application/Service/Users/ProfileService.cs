using FluentResults;
using ResumeSystemManagement.Application.DTOs.Attributes;
using ResumeSystemManagement.Application.DTOs.User;
using ResumeSystemManagement.Application.Interfaces.Auth;
using ResumeSystemManagement.Application.Interfaces.Users;
using ResumeSystemManagement.Application.Mappers;
using ResumeSystemManagement.Core.Interfaces.Repositories.AttributeRepository;

namespace ResumeSystemManagement.Application.Service.Users;

public class ProfileService(
    ICandidateAttributeRepository candidateAttributeRepository,
    IUserContext userContext,
    IAttributeLibraryRepository libraryRepository) : IProfileService
{
    private readonly ICandidateAttributeRepository _candidateAttributeRepository = candidateAttributeRepository;
    private readonly IAttributeLibraryRepository _libraryRepository = libraryRepository;
    private readonly IUserContext _userContext = userContext;

    public async Task<ProfileDetail> GetProfileDetails(string userId)
    {
        var meSectorAttributes = await _candidateAttributeRepository.GetCandidateAttributeValuesBuildIn(userId);
        var infoSectorAttributes = await _candidateAttributeRepository.GetCandidateAttributeValues(userId);
        return new ProfileDetail
        {
            MeSectorAttributes = [.. meSectorAttributes.Select(a => a.ToUserAttributeValue(_userContext.UserId.ToString()))],
            InfoSectorAttributes = [.. infoSectorAttributes.Select(cav => cav.ToUserAttributeValue(_userContext.UserId.ToString()))]
        };
    }

    public Task<Result> UpdateMeSector(List<UserAttributeValue> sectorValues)
    {
        var candidateAttributes = sectorValues
            .Select(sv => sv.MapToCandidateAttributeValue()).ToList();
        return Result.Try(() => _candidateAttributeRepository.UpdateCandidateAttributeValue(candidateAttributes));
    }
    
    public async Task<List<AttributeTemplate>> GetAttributeTemplate(int page)
    {
        var attributes = await _libraryRepository.GetAttributesAsync(page);
        return attributes.Select(a => a.AttributeDetailToTemplate()).ToList();
    }

    public async Task<UserAttributeValue?> GetAttributeValueTemplate(int attributeId, string userId)
    {
        var attribute = await _libraryRepository.GetAttributeByIdAsync(attributeId);
        if (attribute is null)
            return null;

        return new UserAttributeValue(
            0,
            userId,
            attribute.Id,
            attribute.Title,
            attribute.AttributeType.Title,
            string.Empty,
            [.. attribute.AttributeValueForLists.Select(option => new DropdownOption(option.Id, option.Value))],
            0);
    }
}