using ResumeSystemManagement.Application.DTOs.Attributes;
using ResumeSystemManagement.Core.Entities;

namespace ResumeSystemManagement.Application.Mappers;

public static class CandidateAttributeMapper
{
    public static UserAttributeValue ToUserAttributeValue(this CandidateAttributeValue values, string userId)
    {
        return new UserAttributeValue(
            values.Id,
            userId,
            values.Attribute.Id,
            values.Attribute.Title,
            values.Attribute.AttributeType.Title,
            values.Value,
            [.. values.Attribute.AttributeValueForLists.Select(a => new DropdownOption(a.Id, a.Value))],
            values.Version
        );
    }
    
    public static CandidateAttributeValue MapToCandidateAttributeValue(this UserAttributeValue field)
    {
        CandidateAttributeValue candidateValue = new (field.UserId, field.AttributeId, field.Value)
            {
                Id = field.Id,
                Version = field.Version
            };
        return candidateValue;
    }
    
    
    public static CandidateAttributeValue ToAttributeValue(this AttributeTemplate attribute, string userId)
    {
        return new(userId,attribute.Id,string.Empty);
    }
}