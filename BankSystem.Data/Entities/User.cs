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
    
    public virtual List<PasswordKey> PasswordKeys { get; set; }
    
    [Required]
    public bool IsLocked { get; set; } = false;
    
    [Required]
    public DateTime LockedUntil { get; set; } = DateTime.UtcNow;
    
    [Required]
    [RegularExpression("^[a-zA-ZĘęÓóĄąŚśŁłŻżŹźĆćŃń ,.'-]+$")]
    public string FirstName { get; set; } = string.Empty;
    
    [Required]
    [RegularExpression("^[a-zA-ZĘęÓóĄąŚśŁłŻżŹźĆćŃń ,.'-]+$")]
    public string LastName { get; set; } = string.Empty;
    
    public virtual UserSensitiveData SensitiveData { get; set; }
    
    public virtual List<Log> Logins { get; set; }
    
    public virtual BankAccount BankAccount { get; set; }
}