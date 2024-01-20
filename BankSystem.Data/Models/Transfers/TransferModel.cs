namespace BankSystem.Data.Models.Transfers;

public class TransferModel
{
    public Guid Id { get; set; }
    
    public Guid SenderId { get; set; }
    
    public Guid ReceiverId { get; set; }
    
    public string ReceiverName { get; set; } = string.Empty;
    
    public decimal Amount { get; set; }
    
    public string Title { get; set; } = string.Empty;
    
    public DateTime Timestamp { get; set; }
}