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
    public DbSet<BankAccount> BankAccounts { get; set; } = null!;
    public DbSet<PasswordKey> PasswordKeys { get; set; } = null!;
    public DbSet<LoginRequest> LoginRequests { get; set; } = null!;
    public DbSet<Deposit> Deposits { get; set; } = null!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(e => e.Email).IsUnique();
        });

        modelBuilder.Entity<Transfer>(entity =>
        {
            entity.HasOne(x => x.Sender).WithMany(x => x.TransfersSent);
            entity.HasOne(x => x.Receiver).WithMany(x => x.TransfersReceived);
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