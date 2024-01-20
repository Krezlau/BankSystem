﻿using System.ComponentModel.DataAnnotations;

namespace BankSystem.Data.Models;

public class RegisterRequestModel
{
    # region User entity
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
    [Length(24, 24)]
    public string Password { get; set; } = string.Empty;
    
    # endregion
    
    # region UserSensitiveData entity
    
    [Required]
    [RegularExpression(@"^[A-Z]{3}\s\d{6}$")]
    public string IdNumber { get; set; } = string.Empty;
    
    [Required]
    [Phone]
    public string PhoneNumber { get; set; } = string.Empty;
    
    # endregion
}