using System.ComponentModel.DataAnnotations;
namespace ResumeSystemManagement.Web.ViewModels;

public class RegistrationViewModel
{
    [Required] 
    [StringLength(150)]
    [RegularExpression("^[A-Za-zА]+(?:[ ][A-Za-zА]+)+$", ErrorMessage = "Please enter both first and last name")]
    public string FullName { get; set; } = null!;

    [Required]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid pattern.")]
    public string Email { get; set; } = null!;
    
    [Required]
    [StringLength(8,MinimumLength = 1, ErrorMessage = "Password must be at least 1 characters")]
    public string Password { get; set; } = null!;
    
    [Required(ErrorMessage = "You don't repeat password")]
    [Compare(nameof(Password), ErrorMessage = "Passwords do not match")]
    [StringLength(8,MinimumLength = 1, ErrorMessage = "Password must be at least 1 characters")]
    public string ConfirmPassword { get; set; } = null!;
}