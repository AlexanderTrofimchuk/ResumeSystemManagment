using System.ComponentModel.DataAnnotations;
namespace ResumeSystemManagement.Web.ViewModels;

public class RegistrationViewModel
{
    [Required(ErrorMessage = "FullNameRequired")]
    [StringLength(150, ErrorMessage = "FullNameMaxLength")]
    [RegularExpression("^[A-Za-zА]+(?:[ ][A-Za-zА]+)+$", ErrorMessage = "FullNamePattern")]
    public string FullName { get; set; } = null!;

    [Required(ErrorMessage = "EmailRequired")]
    [EmailAddress(ErrorMessage = "EmailInvalid")]
    [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "EmailPatternInvalid")]
    public string Email { get; set; } = null!;
    
    [Required(ErrorMessage = "PasswordRequired")]
    [StringLength(12, ErrorMessage = "PasswordMaxLength")]
    public string Password { get; set; } = null!;
    
    [Required(ErrorMessage = "ConfirmPasswordRequired")]
    [Compare(nameof(Password), ErrorMessage = "PasswordsDoNotMatch")]
    [StringLength(12, ErrorMessage = "PasswordMaxLength")]
    public string ConfirmPassword { get; set; } = null!;
}