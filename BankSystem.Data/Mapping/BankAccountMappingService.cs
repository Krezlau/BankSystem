using BankSystem.Data.Entities;
using BankSystem.Data.Models.Users;

namespace BankSystem.Data.Mapping;

public static class BankAccountMappingService
{
    public static BankAccountModel ToModel(this BankAccount bankAccount)
    {
        return new()
        {
            AccountBalance = bankAccount.AccountBalance,
            AccountNumber = bankAccount.AccountNumber,
            CardNumber = bankAccount.CardNumber,
            Cvv = bankAccount.Cvv,
            ExpirationDate = bankAccount.ExpirationDate
        };
    }
    
}