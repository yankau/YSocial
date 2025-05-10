using System.ComponentModel.DataAnnotations;

namespace YSocial.Components.Models;

public class FriendRequestModel
{
    [Required(ErrorMessage = "Username is required")]
    [StringLength(50, ErrorMessage = "Username must be less than 50 characters")]
    public string Username { get; set; } = string.Empty;
}