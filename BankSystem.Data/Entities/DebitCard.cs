using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankSystem.Data.Entities;

public class DebitCard : Auditable
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    
    [Required]
    public Guid UserId { get; set; }
    
    [ForeignKey(nameof(UserId))]
    public virtual User? User { get; set; }
    
    [Required]
    [MaxLength(16)]
    public string CardNumber { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(3)]
    public string Cvv { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(6)]
    public string ExpirationDate { get; set; } = string.Empty;
}