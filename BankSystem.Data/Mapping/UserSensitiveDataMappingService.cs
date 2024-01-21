using BankSystem.Data.Entities;
using BankSystem.Data.Models.Users;

namespace BankSystem.Data.Mapping;

public static class UserSensitiveDataMappingService
{
    public static UserSensitiveDataModel ToModel(this UserSensitiveData userSensitiveData)
    {
        return new()
        {
            IdNumber = userSensitiveData.IdNumber,
            PhoneNumber = userSensitiveData.PhoneNumber
        };
    }
}