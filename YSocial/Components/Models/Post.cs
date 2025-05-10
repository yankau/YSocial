using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YSocial.Components.Models
{
    [Table("posts")]
    public class Post
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("title")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [Column("content")]
        public string Content { get; set; } = string.Empty;

        [Column("image_urls")]
        public string? ImageUrls { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("user_id")]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual UserAccount User { get; set; } = null!;
    }
}