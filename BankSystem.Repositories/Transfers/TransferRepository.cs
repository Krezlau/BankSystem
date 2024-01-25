using BankSystem.Data;
using BankSystem.Data.Entities;
using BankSystem.Data.Models.Transfers;
using Microsoft.EntityFrameworkCore;

namespace BankSystem.Repositories.Transfers;

public interface ITransferRepository
{
    Task<List<Transfer>> GetHistoryAsync(Guid userId);
    
    Task SendAsync(Guid userId, TransferDefaultModel entity);
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
        // return await _dbContext.Transfers
        //     .Where(x => x.Sender.UserId == userId || x.Receiver.UserId == userId)
        //     .OrderByDescending(x => x.CreatedAt)
        //     .ToListAsync();
        
        var accountNumber = await _dbContext.BankAccounts.Where(x => x.UserId == userId).Select(x => x.Id).FirstOrDefaultAsync();
        
        return await _dbContext.Transfers
            .Where(x => x.SenderId == accountNumber.ToString() || x.ReceiverId == accountNumber.ToString())
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }

    public async Task SendAsync(Guid userId, TransferDefaultModel entity)
    {
        await using var transaction = await _dbContext.Database.BeginTransactionAsync();
        
        _dbContext.Transfers.Add(TransferDefaultModel.ToEntity(entity));
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