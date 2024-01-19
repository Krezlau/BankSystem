using System.ComponentModel.DataAnnotations;

namespace BankSystem.Data.Models;

public class RegisterRequestModel
{
    [Required]
    [MaxLength(50)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    [RegularExpression("^[a-zA-ZĘęÓóĄąŚśŁłŻżŹźĆćŃń ,.'-]+$")]
    public string FirstName { get; set; } = string.Empty;
    
    [Required]
    [RegularExpression("^[a-zA-ZĘęÓóĄąŚśŁłŻżŹźĆćŃń ,.'-]+$")]
    public string LastName { get; set; } = string.Empty;
    
    [Required]
    [MinLength(8)]
    [MaxLength(50)]
    public string Password { get; set; } = string.Empty;
}