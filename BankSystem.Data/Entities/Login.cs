using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankSystem.Data.Entities;

public class Login : Auditable
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    
    [Required]
    public Guid UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public virtual User? User { get; set; } 
    
    [Required]
    public DateTime Timestamp { get; set; }
    
    [Required]
    public bool Successful { get; set; }

    [Required]
    [MaxLength(15)]
    public string IpAddress { get; set; } = string.Empty;
    
    [Required]
    public string UserAgent { get; set; } = string.Empty;
    
    [Required]
    public string Device { get; set; } = string.Empty;
    
    [Required]
    public string Location { get; set; } = string.Empty;
    
    [Required]
    public string Os { get; set; } = string.Empty;
    
    [Required]
    public string Browser { get; set; } = string.Empty;
    
    [Required]
    public string BrowserVersion { get; set; } = string.Empty;
}