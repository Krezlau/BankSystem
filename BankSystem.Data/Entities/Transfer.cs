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
    [Encrypted]
    public string SenderId { get; set; }
    
    [Required]
    [MaxLength(50)]
    [Encrypted]
    public string ReceiverName { get; set; } = string.Empty;
    
    [Required]
    [Encrypted]
    public string ReceiverId { get; set; }
    
    [Required]
    public decimal Amount { get; set; }

    [Required] 
    [MaxLength(25)] 
    public string Title { get; set; } = string.Empty;
}