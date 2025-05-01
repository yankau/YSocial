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
    [MaxLength(50)]
    public string? Username { get; set; }
    
    [Column("password")]
    [MaxLength(100)]
    public string? Password { get; set; }
    
}