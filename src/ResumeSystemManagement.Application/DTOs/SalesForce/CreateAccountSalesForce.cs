namespace ResumeSystemManagement.Application.DTOs.SalesForce;

public class CreateAccountSalesForce
{
    public AccountInfo Account { get; set; } = null!;
    public ContactInfo Contact { get; set; } = null!;
}