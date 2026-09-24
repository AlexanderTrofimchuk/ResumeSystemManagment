using System.ComponentModel.DataAnnotations;

namespace ResumeSystemManagement.Web.ViewModels;

public class LoginViewModel
{
    [Required]
    [EmailAddress(ErrorMessage = "Invalid email address.")] 
    [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid pattern.")]
    public string Email { get; set; } = null!;

    [Required]
    [DataType(DataType.Password)]
    [StringLength(12, ErrorMessage = "Password must be no more than 12 characters")]
    public string Password { get; set; } = null!;
    
    public ChangePasswordViewModel? ChangePassword { get; set; }
}