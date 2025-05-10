using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YSocial.Components.Models;

[Table("chat_messages")]
public class ChatMessage
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("sender_id")]
    public int SenderId { get; set; }

    [Required]
    [Column("receiver_id")]
    public int ReceiverId { get; set; }

    [Required]
    [Column("content")]
    public string Content { get; set; } = string.Empty;

    [Column("timestamp")]
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    [Column("is_read")]
    public bool IsRead { get; set; } = false;

    // Навигационные свойства
    [ForeignKey("SenderId")]
    public virtual UserAccount Sender { get; set; } = null!;

    [ForeignKey("ReceiverId")]
    public virtual UserAccount Receiver { get; set; } = null!;
}