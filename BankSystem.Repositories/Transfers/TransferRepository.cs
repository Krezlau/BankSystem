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
            .Where(x => x.SenderId == userId || x.ReceiverId == userId)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }

    public async Task SendAsync(Guid userId, Transfer entity)
    {
        _dbContext.Transfers.Add(entity);
        await _dbContext.SaveChangesAsync();
    }
}