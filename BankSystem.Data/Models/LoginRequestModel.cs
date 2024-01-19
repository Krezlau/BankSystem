using System.ComponentModel.DataAnnotations;

namespace BankSystem.Data.Models;

public class LoginRequestModel
{
    [Required]
    [MaxLength(50)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    public List<int> PasswordPositions { get; set; } = new List<int>();
    
    [Required]
    public List<char> PasswordCharacters { get; set; } = new List<char>();
}