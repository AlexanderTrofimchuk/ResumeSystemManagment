namespace ResumeSystemManagement.Application.DTOs.SalesForce;

public class ContactInfo
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? Email { get; set; } = null;
    public string? Phone { get; set; } = null;
    
    public string? AccountId { get; set; } = null;
}