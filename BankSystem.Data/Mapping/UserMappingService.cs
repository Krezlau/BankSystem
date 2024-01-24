using BankSystem.Data.Entities;
using BankSystem.Data.Models;

namespace BankSystem.Data.Mapping;

public static class UserMappingService
{
    public static User ToEntity(this RegisterRequestModel model, (string hashedSecret, List<PasswordKey> keys) partialPassword)
    {
        return new User
        {
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName,
            SecretHash = partialPassword.hashedSecret,
            PasswordKeys = partialPassword.keys,
            SensitiveData = new UserSensitiveData
            {
                IdNumber = model.IdNumber,
                PhoneNumber = model.PhoneNumber
            },
            BankAccount = GenerateBankAccountWithoutCardNumber()
        };
    }
    
    private static BankAccount GenerateBankAccountWithoutCardNumber()
    {
        var random = new Random();
        var cvv = new string(Enumerable.Range(1, 3)
            .Select(_ => (char)random.Next('0', '9'))
            .ToArray());
        var expirationDate = DateTime.UtcNow.AddYears(5).ToString("MM/yy");
        return new BankAccount
        {
            CardNumber = "1234",
            Cvv = cvv,
            ExpirationDate = expirationDate,
            AccountBalance = 100
        };
    }
}