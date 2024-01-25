using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;
using System.Text;

namespace BankSystem.Data.Entities;

public class PasswordKey 
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public string Salt { get; set; }
    
    [Required]
    public string EncryptedShare { get; set; }
    
    [Required]
    public string IV { get; set; }
    
    [Required]
    public Guid UserId { get; set; }

    [ForeignKey(nameof(UserId))] 
    public virtual User User { get; set; } = new User();
}