namespace YSocial.Components.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("friendships")]
public class Friendship
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("user1_id")]
    public int User1Id { get; set; }

    [Required]
    [Column("user2_id")]
    public int User2Id { get; set; }

    [Required]
    [Column("status")]
    public string Status { get; set; } = "pending"; // "pending" or "accepted"

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey("User1Id")]
    public virtual UserAccount User1 { get; set; } = null!;

    [ForeignKey("User2Id")]
    public virtual UserAccount User2 { get; set; } = null!;
}