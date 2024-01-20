using BankSystem.Data.Mapping;
using BankSystem.Data.Models.Transfers;
using BankSystem.Repositories.BankAccounts;
using BankSystem.Repositories.Transfers;

namespace BankSystem.Services.Transfers;

public interface ITransferService
{
    Task<decimal> SendAsync(Guid userId, TransferSendModel model);
    
    Task<List<TransferModel>> GetHistoryAsync(Guid userId);
}

public class TransferService : ITransferService
{
    private readonly ITransferRepository _transferRepository;
    private readonly IBankAccountRepository _bankAccountRepository;

    public TransferService(ITransferRepository transferRepository, IBankAccountRepository bankAccountRepository)
    {
        _transferRepository = transferRepository;
        _bankAccountRepository = bankAccountRepository;
    }

    public async Task<decimal> SendAsync(Guid userId, TransferSendModel model)
    {
        var (recipient, sender) = await _bankAccountRepository.GetBankAccountsAsync(model.RecipientBankAccountNumber, userId);

        if (recipient is null || sender is null)
        {
            throw new ArgumentException("Invalid bank account number.");
        }

        if (sender.AccountBalance < model.Amount)
        {
            throw new ArgumentException("Insufficient funds.");
        }

        var transfer = model.ToEntity(sender.Id, recipient.Id);

        await _transferRepository.SendAsync(userId, transfer);
        
        return sender.AccountBalance;
    }

    public async Task<List<TransferModel>> GetHistoryAsync(Guid userId)
    {
        var transfers = await _transferRepository.GetHistoryAsync(userId);

        return transfers.Select(x => x.ToModel()).ToList();
    }
}