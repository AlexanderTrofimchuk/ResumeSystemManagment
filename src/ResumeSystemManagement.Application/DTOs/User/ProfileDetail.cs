using ResumeSystemManagement.Application.DTOs.Attributes;

namespace ResumeSystemManagement.Application.DTOs.User;

public class ProfileDetail
{
    public List<UserAttributeValue> MeSectorAttributes { get; set;} = new();
    public List<UserAttributeValue> InfoSectorAttributes { get; set;} = new();
}