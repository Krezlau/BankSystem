using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;
using System.Text;

namespace BankSystem.Data.Entities;

public class PasswordKey : Auditable
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public byte[] Salt { get; set; } = new byte[16];
    
    [Required]
    public byte[] EncryptedShare { get; set; } = new byte[32];
    
    [Required]
    public byte[] IV { get; set; } = new byte[16];
    
    [Required]
    public Guid UserId { get; set; }

    [ForeignKey(nameof(UserId))] 
    public virtual User User { get; set; } = new User();
}