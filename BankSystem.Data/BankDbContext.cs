using BankSystem.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankSystem.Data;

public class BankDbContext : DbContext
{
    public BankDbContext(DbContextOptions<BankDbContext> options) : base(options)
    {
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Transfer> Transfers { get; set; }
    public DbSet<UserSensitiveData> UserSensitiveData { get; set; }
    public DbSet<Logins> Logins { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();
    }
}