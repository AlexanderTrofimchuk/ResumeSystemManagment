using FluentResults;
using ResumeSystemManagement.Application.DTOs.Attributes;
using ResumeSystemManagement.Application.DTOs.User;

namespace ResumeSystemManagement.Application.Interfaces.Users;

public interface IProfileService
{
    Task<ProfileDetail> GetProfileDetails(string userId);
    Task<List<AttributeTemplate>> GetAttributeTemplate(int page);
    Task<UserAttributeValue?> GetAttributeValueTemplate(int attributeId, string userId);
    Task<Result> UpdateMeSector(List<UserAttributeValue> sectorValues);
    Task<Result> UpdateInfoSector(List<UserAttributeValue> sectorValues);
    Task<Result<bool>> AddAttribute(AttributeTemplate attribute);
    Task<Result<bool>> DeleteAttributeInfo(List<int> ids);
}