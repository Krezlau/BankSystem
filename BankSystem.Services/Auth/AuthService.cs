using System.Text;
using BankSystem.Data.Entities;
using BankSystem.Data.Models;
using BankSystem.Repositories.Auth;
using BankSystem.Repositories.Users;

namespace BankSystem.Services.Auth;

public interface IAuthService
{
    Task<LoginCheckResponseModel> LoginCheckAsync(LoginCheckRequestModel model);
    
    Task<bool> LoginAsync(LoginRequestModel model);
    
    Task<bool> RegisterAsync(RegisterRequestModel model);
}


public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly ILoginRequestRepository _loginRequestRepository;

    public AuthService(IUserRepository userRepository, ILoginRequestRepository loginRequestRepository)
    {
        _userRepository = userRepository;
        _loginRequestRepository = loginRequestRepository;
    }

    public async Task<LoginCheckResponseModel> LoginCheckAsync(LoginCheckRequestModel model)
    {
        var mask = GenerateMask();
        
        var loginRequest = await _loginRequestRepository.CreateLoginRequestAsync(mask, model.Email);
        
        return new LoginCheckResponseModel(mask, loginRequest.Id);
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
    
    # region private methods

    private static string GenerateMask()
    {
        var random = new Random();
        HashSet<int> positions = new();
        while (positions.Count < 5)
        {
            positions.Add(random.Next(0, 24));
        }
        
        
        var mask = new StringBuilder();
        for (var i = 0; i < 24; i++)
        {
            mask.Append(positions.Contains(i) ? '1' : '0');
        }
        return mask.ToString();
    }
    
    # endregion
}