using ResumeSystemManagement.Core.Entities;

namespace ResumeSystemManagement.Core.Interfaces.Repositories.AttributeRepository;

public interface ICandidateAttributeRepository
{
    Task<List<CandidateAttributeValue>> GetAttributeValues(string userId);
    Task<List<CandidateAttributeValue>> GetBuildInAttribute(string userId);
    Task InitialBuildInAttributes(string userId, string username);
    Task<bool> CreateAttributeValue(CandidateAttributeValue candidateAttributeValue);
    Task UpdateAttributeValue(List<CandidateAttributeValue> candidateAttributes);
    Task UpdateInfoAttributeValues(List<CandidateAttributeValue> candidateAttributes, string userId);
    Task<bool> DeleteAttributeValues(List<int> ids, string userId);
}