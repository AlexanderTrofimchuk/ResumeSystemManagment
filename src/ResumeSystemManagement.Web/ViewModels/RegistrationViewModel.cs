using System.ComponentModel.DataAnnotations;
namespace ResumeSystemManagement.Web.ViewModels;

public class RegistrationViewModel
{
    [Required(ErrorMessage = "Full name is required.")] 
    [StringLength(150)]
    [RegularExpression("^[A-Za-zА]+(?:[ ][A-Za-zА]+)+$", ErrorMessage = "Please enter both first and last name")]
    public string FullName { get; set; } = null!;

    [Required(ErrorMessage = "Email address is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid pattern.")]
    public string Email { get; set; } = null!;
    
    [Required(ErrorMessage = "Password is required.")]
    [StringLength(12,ErrorMessage = "Password must be at least 1 characters")]
    public string Password { get; set; } = null!;
    
    [Required(ErrorMessage = "You don't repeat password")]
    [Compare(nameof(Password), ErrorMessage = "Passwords do not match")]
    [StringLength(12, ErrorMessage = "Password must be at least 1 characters")]
    public string ConfirmPassword { get; set; } = null!;
}