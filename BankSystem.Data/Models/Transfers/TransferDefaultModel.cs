using BankSystem.Data.Entities;

namespace BankSystem.Data.Models.Transfers;

public class TransferDefaultModel : Auditable
{
    public Guid Id { get; set; }
    
    public Guid SenderId { get; set; }
    
    public string ReceiverName { get; set; } = string.Empty;
    
    public Guid ReceiverId { get; set; }
    
    public decimal Amount { get; set; }

    public string Title { get; set; } = string.Empty;
    
    public static TransferDefaultModel FromEntity(Transfer transfer)
    {
        return new()
        {
            Id = transfer.Id,
            SenderId = Guid.Parse(transfer.SenderId),
            ReceiverName = transfer.ReceiverName,
            ReceiverId = Guid.Parse(transfer.ReceiverId),
            Amount = transfer.Amount,
            Title = transfer.Title,
            CreatedAt = transfer.CreatedAt,
            UpdatedAt = transfer.UpdatedAt
        };
    }
    
    public static Transfer ToEntity(TransferDefaultModel transfer)
    {
        return new()
        {
            Id = transfer.Id,
            SenderId = transfer.SenderId.ToString(),
            ReceiverName = transfer.ReceiverName,
            ReceiverId = transfer.ReceiverId.ToString(),
            Amount = transfer.Amount,
            Title = transfer.Title,
            CreatedAt = transfer.CreatedAt,
            UpdatedAt = transfer.UpdatedAt
        };
    }
}
