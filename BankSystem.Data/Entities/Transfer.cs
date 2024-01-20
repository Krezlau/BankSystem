using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BankSystem.Data.Entities;

public class Transfer : Auditable
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    
    [Required]
    public Guid SenderId { get; set; }
    
    [ForeignKey(nameof(SenderId))]
    [DeleteBehavior(DeleteBehavior.NoAction)]
    public virtual BankAccount? Sender { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string ReceiverName { get; set; } = string.Empty;
    
    [Required]
    public Guid ReceiverId { get; set; }
    
    [ForeignKey(nameof(ReceiverId))]
    [DeleteBehavior(DeleteBehavior.NoAction)]
    public virtual BankAccount? Receiver { get; set; }
    
    [Required]
    public decimal Amount { get; set; }

    [Required] 
    [MaxLength(25)] 
    public string Title { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(15)]
    public string? IpAddress { get; set; }
}