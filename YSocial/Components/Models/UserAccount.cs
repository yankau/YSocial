using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YSocial.Components.Models;

[Table("users")]
public class UserAccount
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("username")]
    public string Username { get; set; } = string.Empty;

    [Required]
    [Column("password")]
    public string Password { get; set; } = string.Empty;

    [Column("avatar_url")]
    public string? AvatarUrl { get; set; }

    [Column("description")]
    public string? Description { get; set; }

    public virtual List<Post> Posts { get; set; } = new();
}