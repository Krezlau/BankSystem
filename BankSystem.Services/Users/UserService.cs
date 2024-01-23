using BankSystem.Data.Mapping;
using BankSystem.Data.Models.Users;
using BankSystem.Repositories.BankAccounts;
using BankSystem.Repositories.Users;

namespace BankSystem.Services.Users;

public interface IUserService
{
    Task<BankAccountModel> GetMyAccountAsync(Guid userId);
    
    Task<UserSensitiveDataModel> GetMySensitiveDataAsync(Guid userId);
}

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IBankAccountRepository _bankAccountRepository;

    public UserService(IUserRepository userRepository, IBankAccountRepository bankAccountRepository)
    {
        _userRepository = userRepository;
        _bankAccountRepository = bankAccountRepository;
    }

    public async Task<BankAccountModel> GetMyAccountAsync(Guid userId)
    {
        var bankAccount = await _bankAccountRepository.GetBankAccountAsync(userId);

        return bankAccount.ToModel();
    }

    public async Task<UserSensitiveDataModel> GetMySensitiveDataAsync(Guid userId)
    {
        var user = await _userRepository.GetUserAsync(userId);
        if (user is null) throw new KeyNotFoundException("User not found");
        
        var sensitiveData = await _userRepository.GetUserSensitiveDataAsync(userId);
        if (sensitiveData is null) throw new KeyNotFoundException("User sensitive data not found");

        return sensitiveData.ToModel(user.FirstName, user.LastName);
    }
}