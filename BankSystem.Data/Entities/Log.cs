using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankSystem.Data.Entities;

public class Log : Auditable
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public Guid? UserId { get; set; } = null;

    [Required]
    public bool Successful { get; set; }

    [Required]
    public string IpAddress { get; set; } = string.Empty;
    
    [Required]
    public string Url { get; set; } = string.Empty;
    
    [Required]
    public string UserAgent { get; set; } = string.Empty;
}