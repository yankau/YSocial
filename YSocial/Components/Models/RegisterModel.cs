using System.ComponentModel.DataAnnotations;

namespace YSocial.Components.Models;

public class RegisterModel
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "Username is requiredp")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be 3-50 characters")]
    public string Username { get; set; } 

    [Required(AllowEmptyStrings = false,ErrorMessage = "Password is required")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters")]
    public string Password { get; set; } 

    [Required(AllowEmptyStrings = false,ErrorMessage = "Please confirm your password")]
    [Compare(nameof(Password), ErrorMessage = "Passwords do not match")]
    public string ConfirmPassword { get; set; } 
}