using System.Text;
using BankSystem.Data;
using BankSystem.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankSystem.Repositories.BankAccounts;

public interface IBankAccountRepository
{
    Task<(BankAccount? recipient, BankAccount? sender)> GetBankAccountsAsync(string recipientBankAccountNumber, Guid senderId);

    Task GiveUserOneHundredPLNAsync(BankAccount bankAccount);
    
    Task AssignCardNumberAsync(BankAccount bankAccount);
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
            .Include(x => x.Deposits)
            .Include(x => x.TransfersReceived)
            .Include(x => x.TransfersSent)
            .FirstOrDefaultAsync(x => x.AccountNumber == recipientBankAccountNumber);

        var sender = await _dbContext.BankAccounts
            .Include(x => x.Deposits)
            .Include(x => x.TransfersReceived)
            .Include(x => x.TransfersSent)
            .FirstOrDefaultAsync(x => x.UserId == senderId);

        return (recipient, sender);
    }

    public async Task GiveUserOneHundredPLNAsync(BankAccount bankAccount)
    {
        var deposit = new Deposit
        {
            Amount = 100,
            BankAccountId = bankAccount.Id
        };
        _dbContext.Deposits.Add(deposit);
        await _dbContext.SaveChangesAsync();
        
    }

    public async Task AssignCardNumberAsync(BankAccount bankAccount)
    {
        var cardNumber = GenerateCardNumber();
        while (await _dbContext.BankAccounts.AnyAsync(x => x.CardNumber == cardNumber))
        {
            cardNumber = GenerateCardNumber();
        }

        bankAccount.CardNumber = cardNumber;
        _dbContext.BankAccounts.Update(bankAccount);
        await _dbContext.SaveChangesAsync();
    }
    
    private static string GenerateCardNumber()
    {
        var random = new Random();
        return new string(Enumerable.Range(1, 16)
            .Select(_ => (char)random.Next('0', '9'))
            .ToArray());
    }
}