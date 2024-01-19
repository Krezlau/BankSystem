using System.ComponentModel.DataAnnotations;

namespace BankSystem.Data.Models;

public class LoginRequestModel
{
    [Required]
    [MaxLength(50)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    public Guid Key { get; set; }
    
    [Required]
    public string PasswordCharacters { get; set; } = string.Empty;
}