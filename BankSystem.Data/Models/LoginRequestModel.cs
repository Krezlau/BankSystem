using System.ComponentModel.DataAnnotations;

namespace BankSystem.Data.Models;

public class LoginRequestModel
{
    [Required]
    [MaxLength(50)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    public List<(int pos, char c)> PasswordChars { get; set; } = new();
}