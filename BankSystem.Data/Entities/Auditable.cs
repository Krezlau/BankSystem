﻿using System.ComponentModel.DataAnnotations.Schema;

namespace BankSystem.Data.Entities;

public abstract class Auditable
{
    public DateTime CreatedAt { get; set; }
    
    public DateTime? UpdatedAt { get; set; }
}