using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using YSocial.Components.Models;

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

    [Column("is_online")]
    public bool IsOnline { get; set; } = false;

    [Column("last_seen")]
    public DateTime LastSeen { get; set; } = DateTime.UtcNow;

    public virtual List<Post> Posts { get; set; } = new();

    // Navigation properties for friendships
    public virtual List<Friendship> FriendshipsAsUser1 { get; set; } = new();
    public virtual List<Friendship> FriendshipsAsUser2 { get; set; } = new();
}