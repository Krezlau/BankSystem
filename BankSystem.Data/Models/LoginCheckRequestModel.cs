using System.ComponentModel.DataAnnotations;

namespace BankSystem.Data.Models;

public class LoginCheckRequestModel
{
    [Required]
    [MaxLength(50)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty; 
}