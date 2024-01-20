using System.ComponentModel.DataAnnotations;

namespace BankSystem.Data.Entities;

public class LoginRequest : Auditable
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    [MaxLength(24)]
    public string Mask { get; set; } = string.Empty;
    
    [Required]
    public DateTime ExpiresAt { get; set; } = DateTime.UtcNow.AddMinutes(10);
    
    [Required]
    public bool Consumed { get; set; } = false;
    
    [Required]
    public bool Failed { get; set; } = false;
    
    [Required]
    public string Email { get; set; } = string.Empty;
}