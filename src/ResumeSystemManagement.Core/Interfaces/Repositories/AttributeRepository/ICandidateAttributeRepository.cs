using ResumeSystemManagement.Core.Entities;

namespace ResumeSystemManagement.Core.Interfaces.Repositories.AttributeRepository;

public interface ICandidateAttributeRepository
{
    Task<List<CandidateAttributeValue>> GetCandidateAttributeValues(string userId);
    Task<List<CandidateAttributeValue>> GetCandidateAttributeValuesBuildIn(string userId);
    Task InitialBuildInAttributes(string userId, string username);
    Task CreateCandidateAttributeValue(CandidateAttributeValue candidateAttributeValue);
    Task UpdateCandidateAttributeValue(List<CandidateAttributeValue> candidateAttributes);
}