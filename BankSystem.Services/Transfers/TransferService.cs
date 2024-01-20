using BankSystem.Data.Models.Transfers;

namespace BankSystem.Services.Transfers;

public interface ITransferService
{
    Task<decimal> SendAsync(Guid userId, TransferSendModel model);
    
    Task<List<TransferModel>> GetHistoryAsync(Guid userId);
}

public class TransferService : ITransferService
{
    public async Task<decimal> SendAsync(Guid userId, TransferSendModel model)
    {
        return 0m;
    }

    public async Task<List<TransferModel>> GetHistoryAsync(Guid userId)
    {
        throw new NotImplementedException();
    }
}