using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BankSystem.Data.Entities;

public class Deposit : Auditable
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    
    [Required]
    public Guid BankAccountId { get; set; }
    
    [ForeignKey(nameof(BankAccountId))]
    [DeleteBehavior(DeleteBehavior.NoAction)]
    public virtual BankAccount? BankAccount { get; set; }
    
    [Required]
    public decimal Amount { get; set; }
}