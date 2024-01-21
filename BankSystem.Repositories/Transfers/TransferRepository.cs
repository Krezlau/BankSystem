using BankSystem.Data;
using BankSystem.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankSystem.Repositories.Transfers;

public interface ITransferRepository
{
    Task<List<Transfer>> GetHistoryAsync(Guid userId);
    
    Task SendAsync(Guid userId, Transfer entity);
}

public class TransferRepository : ITransferRepository
{
    private readonly BankDbContext _dbContext;

    public TransferRepository(BankDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Transfer>> GetHistoryAsync(Guid userId)
    {
        return await _dbContext.Transfers
            .Where(x => x.Sender.UserId == userId || x.Receiver.UserId == userId)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }

    public async Task SendAsync(Guid userId, Transfer entity)
    {
        await using var transaction = await _dbContext.Database.BeginTransactionAsync();
        
        _dbContext.Transfers.Add(entity);
        await _dbContext.SaveChangesAsync();
        
        // Update sender's account balance
        var sender = await _dbContext.BankAccounts.Where(x => x.Id == entity.SenderId).FirstOrDefaultAsync();
        sender.AccountBalance -= entity.Amount;
        _dbContext.BankAccounts.Update(sender);
        
        // Update receiver's account balance
        var receiver = await _dbContext.BankAccounts.Where(x => x.Id == entity.ReceiverId).FirstOrDefaultAsync();
        receiver.AccountBalance += entity.Amount;
        _dbContext.BankAccounts.Update(receiver);
        
        await _dbContext.SaveChangesAsync();
        
        await transaction.CommitAsync();
    }
}