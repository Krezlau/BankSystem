using BankSystem.Data.Entities;
using BankSystem.Data.Models;
using BankSystem.Repositories.Users;

namespace BankSystem.Services.Auth;

public interface IAuthService
{
    Task<bool> LoginCheckAsync(LoginCheckRequestModel model);
    
    Task<bool> LoginAsync(LoginRequestModel model);
    
    Task<bool> RegisterAsync(RegisterRequestModel model);
}


public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;

    public AuthService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<bool> LoginCheckAsync(LoginCheckRequestModel model)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> LoginAsync(LoginRequestModel model)
    {
        var user = await _userRepository.GetUserWithPasswordAsync(model.Email);
        if (user is null) return false;
        var passwordChars = model.PasswordCharacters.Select((t, i) => (model.PasswordPositions[i], t)).ToList();
        return PasswordService.VerifyPassword(passwordChars, user.SecretHash, user.PasswordKeys);
    }

    public async Task<bool> RegisterAsync(RegisterRequestModel model)
    {
        var partialPassword = PasswordService.CreatePassword(model.Password);
        var user = new User
        {
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName,
            SecretHash = partialPassword.hashedSecret,
        };
        return await _userRepository.CreateUserAsync(user, partialPassword.keys);
    }
}