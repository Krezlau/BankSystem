using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankSystem.Data.Entities;

public class User : Auditable
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    
    [Required]
    [MaxLength(50)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    public string SecretHash { get; set; } = string.Empty;
    
    public virtual List<PasswordKey> PasswordKeys { get; set; } = new();
    
    [Required]
    [RegularExpression("^[a-zA-ZĘęÓóĄąŚśŁłŻżŹźĆćŃń ,.'-]+$")]
    public string FirstName { get; set; } = string.Empty;
    
    [Required]
    [RegularExpression("^[a-zA-ZĘęÓóĄąŚśŁłŻżŹźĆćŃń ,.'-]+$")]
    public string LastName { get; set; } = string.Empty;
    
    [Required]
    public decimal AccountBalance { get; set; }
    
    public virtual UserSensitiveData SensitiveData { get; set; } = new();
    
    public virtual List<Transfer> TransfersSent { get; set; } = new();
    
    public virtual List<Transfer> TransfersReceived { get; set; } = new();
    
    public virtual List<Login> Logins { get; set; } = new();
    
    public virtual DebitCard DebitCard { get; set; } = new();
}