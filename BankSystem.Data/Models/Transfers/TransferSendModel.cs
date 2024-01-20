using System.ComponentModel.DataAnnotations;

namespace BankSystem.Data.Models.Transfers;

public class TransferSendModel
{
    [Required]
    [RegularExpression("[A-Z0-9]{32}")]
    public string RecipientBankAccountNumber { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(50)]
    public string RecipientFullName { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(25)]
    public string Title { get; set; } = string.Empty;
    
    [Required]
    [Range(0.01, 1_000_000)]
    public decimal Amount { get; set; }
}