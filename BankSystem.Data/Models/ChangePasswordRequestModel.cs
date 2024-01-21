using System.ComponentModel.DataAnnotations;

namespace BankSystem.Data.Models;

public class ChangePasswordRequestModel
{
    [Required]
    public Guid Key { get; set; }
    
    [Required]
    public string PasswordCharacters { get; set; } = string.Empty;
    
    [Required]
    public string NewPassword { get; set; } = string.Empty;
}