using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankSystem.Data.Entities;

public class BankAccount : Auditable
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    
    [Required]
    public Guid UserId { get; set; }
    
    [ForeignKey(nameof(UserId))]
    public virtual User? User { get; set; }
    
    [Required]
    [MaxLength(32)]
    public string AccountNumber
    {
        get => Id.ToString("N").ToUpper();
        protected set { }
    }

    [Required]
    public decimal AccountBalance { get; set; }
    
    [Required]
    [MaxLength(16)]
    public string CardNumber { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(3)]
    public string Cvv { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(6)]
    public string ExpirationDate { get; set; } = string.Empty;

    public virtual List<Transfer> TransfersSent { get; set; } = new();

    public virtual List<Transfer> TransfersReceived { get; set; } = new();
    
    public virtual List<Deposit> Deposits { get; set; } = new();
}