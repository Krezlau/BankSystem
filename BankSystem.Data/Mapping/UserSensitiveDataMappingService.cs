using BankSystem.Data.Entities;
using BankSystem.Data.Models.Users;

namespace BankSystem.Data.Mapping;

public static class UserSensitiveDataMappingService
{
    public static UserSensitiveDataModel ToModel(this UserSensitiveData userSensitiveData, string firstName, string lastName)
    {
        return new()
        {
            FirstName = firstName,
            LastName = lastName,
            IdNumber = userSensitiveData.IdNumber,
            PhoneNumber = userSensitiveData.PhoneNumber
        };
    }
}