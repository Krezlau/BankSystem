﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BankSystem.Data.Entities;

public class Transfer : Auditable
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    
    [Required]
    public Guid SenderId { get; set; }
    
    [ForeignKey(nameof(SenderId))]
    [DeleteBehavior(DeleteBehavior.NoAction)]
    public virtual User? Sender { get; set; }
    
    [Required]
    public Guid ReceiverId { get; set; }
    
    [ForeignKey(nameof(ReceiverId))]
    [DeleteBehavior(DeleteBehavior.NoAction)]
    public virtual User? Receiver { get; set; }
    
    [Required]
    public decimal Amount { get; set; }
    
    [Required]
    public decimal AccountBalanceAfter { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string? Description { get; set; }
    
    [Required]
    public DateTime Timestamp { get; set; }
    
    [Required]
    [MaxLength(15)]
    public string? IpAddress { get; set; }
}