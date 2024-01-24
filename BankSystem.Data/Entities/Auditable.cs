using System.ComponentModel.DataAnnotations.Schema;

namespace BankSystem.Data.Entities;

public abstract class Auditable
{
    // [Column(TypeName = "timestamp without time zone")]
    public DateTime CreatedAt { get; set; }
    
    // [Column(TypeName = "timestamp without time zone")]
    public DateTime? UpdatedAt { get; set; }
}