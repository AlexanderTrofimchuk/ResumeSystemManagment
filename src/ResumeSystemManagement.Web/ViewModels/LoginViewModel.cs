using System.ComponentModel.DataAnnotations;

namespace ResumeSystemManagement.Web.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "EmailRequired")]
    [EmailAddress(ErrorMessage = "EmailInvalid")]
    [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "EmailPatternInvalid")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "PasswordRequired")]
    [DataType(DataType.Password)]
    [StringLength(12, ErrorMessage = "PasswordMaxLength")]
    public string Password { get; set; } = null!;
    
    public ChangePasswordViewModel? ChangePassword { get; set; }
}