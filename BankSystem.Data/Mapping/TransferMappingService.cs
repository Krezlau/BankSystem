using BankSystem.Data.Entities;
using BankSystem.Data.Models.Transfers;

namespace BankSystem.Data.Mapping;

public static class TransferMappingService
{
    public static TransferDefaultModel ToEntity(this TransferSendModel model, Guid senderId, Guid receiverId)
    {
        return new TransferDefaultModel
        {
            Amount = model.Amount,
            Title = model.Title,
            SenderId = senderId,
            ReceiverId = receiverId,
            ReceiverName = model.RecipientFullName
        };
    }
    
    public static TransferModel ToModel(this TransferDefaultModel entity)
    {
        return new TransferModel
        {
            Id = entity.Id,
            Amount = entity.Amount,
            Title = entity.Title,
            SenderId = entity.SenderId,
            ReceiverId = entity.ReceiverId,
            Timestamp = entity.CreatedAt,
            ReceiverName = entity.ReceiverName,
        };
    }
}