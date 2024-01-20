using BankSystem.Data;
using BankSystem.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankSystem.Repositories.BankAccounts;

public interface IBankAccountRepository
{
    Task<(BankAccount? recipient, BankAccount? sender)> GetBankAccountsAsync(string recipientBankAccountNumber, Guid senderId);
}


public class BankAccountRepository : IBankAccountRepository
{
    private readonly BankDbContext _dbContext;

    public BankAccountRepository(BankDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<(BankAccount? recipient, BankAccount? sender)> GetBankAccountsAsync(string recipientBankAccountNumber, Guid senderId)
    {
        var recipient = await _dbContext.BankAccounts
            .FirstOrDefaultAsync(x => x.AccountNumber == recipientBankAccountNumber);

        var sender = await _dbContext.BankAccounts
            .FirstOrDefaultAsync(x => x.UserId == senderId);

        return (recipient, sender);
    }
}