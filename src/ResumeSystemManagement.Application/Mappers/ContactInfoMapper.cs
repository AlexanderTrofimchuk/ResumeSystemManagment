using ResumeSystemManagement.Application.DTOs.SalesForce;
using ResumeSystemManagement.Core.Entities;

namespace ResumeSystemManagement.Application.Mappers;

public static class ContactInfoMapper
{
    public static ContactInfo ToContactInfo(this List<CandidateAttributeValue> candidateAttribute)
    {
        return new()
        {
            FirstName = candidateAttribute
                .FirstOrDefault(x => x.Attribute.Title == "First Name")?
                .Value ?? string.Empty,
            LastName = candidateAttribute
                .FirstOrDefault(x => x.Attribute.Title == "Last Name")?
                .Value ?? string.Empty,
            Email = candidateAttribute
                .FirstOrDefault(x => x.Attribute.Title == nameof(ContactInfo.Email))?
                .Value ?? string.Empty,
            Phone = candidateAttribute
                .FirstOrDefault(x => x.Attribute.Title == nameof(ContactInfo.Phone))?
                .Value ?? string.Empty
        };
    }
}