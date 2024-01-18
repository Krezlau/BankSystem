using BankSystem.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankSystem.Data;

public class BankDbContext : DbContext
{
    public BankDbContext(DbContextOptions<BankDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Transfer> Transfers { get; set; } = null!;
    public DbSet<UserSensitiveData> UserSensitiveData { get; set; } = null!;
    public DbSet<Login> Logins { get; set; } = null!;
    public DbSet<DebitCard> DebitCards { get; set; } = null!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}