using System.ComponentModel.DataAnnotations;

namespace YSocial.Components.Models;

public class LoginModel
{
   [Required(AllowEmptyStrings = false, ErrorMessage = "Username is required")]
    public string Username { get; set; }
    
    [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required"), MinLength(8)]
    public string Password { get; set; }
}