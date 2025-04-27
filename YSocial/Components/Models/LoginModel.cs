using System.ComponentModel.DataAnnotations;

namespace YSocial.Components.Models;

public class LoginModel
{
   // [Required, EmailAddress, DataType(DataType.EmailAddress)]
    public string Username { get; set; }
    
    [Required, MinLength(8)]
    public string Password { get; set; }
}