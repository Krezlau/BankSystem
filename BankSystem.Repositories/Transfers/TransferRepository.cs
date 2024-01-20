using BankSystem.Data;

namespace BankSystem.Repositories.Transfers;

public interface ITransferRepository
{
    
}

public class TransferRepository : ITransferRepository
{
    private readonly BankDbContext _dbContext;

    public TransferRepository(BankDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}