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

    [Column("username")]
    [Required]
    [MaxLength(50)]
    public string? Username { get; set; } = string.Empty;

    [Column("password")] 
    [Required]
    [MaxLength(256)] 
    public string? Password { get; set; } = string.Empty;
    
    [Column("avatar_url")]
    public string AvatarUrl { get; set; } = "/images/default-avatar.png";

    [Column("description")]
    public string Description { get; set; } = string.Empty;

    // Связь с постами
    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
}