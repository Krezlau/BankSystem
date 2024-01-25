using System.Text;
using BankSystem.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.DataEncryption;
using Microsoft.EntityFrameworkCore.DataEncryption.Providers;
using Microsoft.Extensions.Configuration;

namespace BankSystem.Data;

public class BankDbContext : DbContext
{
    private readonly byte[] _encryptionKey;
    private readonly byte[] _encryptionIV;
    private readonly IEncryptionProvider _encryptionProvider;
    
    public BankDbContext(DbContextOptions<BankDbContext> options, IConfiguration config) : base(options)
    {
        _encryptionKey = Encoding.UTF8.GetBytes(config["EncryptionKey"]);
        _encryptionIV = Encoding.UTF8.GetBytes(config["EncryptionIV"]);
        _encryptionProvider = new AesProvider(_encryptionKey, _encryptionIV);
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Transfer> Transfers { get; set; } = null!;
    public DbSet<UserSensitiveData> UserSensitiveData { get; set; } = null!;
    public DbSet<Log> Logins { get; set; } = null!;
    public DbSet<BankAccount> BankAccounts { get; set; } = null!;
    public DbSet<PasswordKey> PasswordKeys { get; set; } = null!;
    public DbSet<LoginRequest> LoginRequests { get; set; } = null!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseEncryption(_encryptionProvider);
        
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(e => e.Email).IsUnique();
        });

        modelBuilder.Entity<BankAccount>(entity =>
        {
            entity.HasIndex(x => x.AccountNumber).IsUnique();
            entity.HasIndex(x => x.CardNumber).IsUnique();
        });
        
         modelBuilder.Entity<BankAccount>()
             .ToTable(b => b.HasCheckConstraint("CK_BankAccounts_Balance", "[AccountBalance] >= 0"));
        
        base.OnModelCreating(modelBuilder);
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }
    
    private void TrackChanges()
    {
        var tracker = ChangeTracker;

        foreach (var entry in tracker.Entries())
        {
            if (entry.Entity is Auditable referenceEntity)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        referenceEntity.CreatedAt = DateTime.Now;
                        break;
                    case EntityState.Modified:
                        referenceEntity.UpdatedAt = DateTime.Now;
                        break;
                    default:
                        break;
                }

            }
        }
    }
    
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        TrackChanges();

        return base.SaveChangesAsync(cancellationToken);
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        TrackChanges();

        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        TrackChanges();

        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    public override int SaveChanges()
    {
        TrackChanges();

        return base.SaveChanges();
    }
}