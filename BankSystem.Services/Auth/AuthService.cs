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
        //var mask = "010101010100000000000000";
        
        // todo consume old login requests
        var loginRequest = await _loginRequestRepository.CreateLoginRequestAsync(mask, model.Email);
        
        return new LoginCheckResponseModel(mask, loginRequest.Id);
    }

    public async Task<bool> LoginAsync(LoginRequestModel model)
    {
        var loginRequest = await ValidateLoginRequestModelAndThrowAsync(model);
        
        var user = await _userRepository.GetUserWithPasswordAsync(model.Email);
        if (user is null) return false;
        var passwordChars = ParseMaskAndCharacters(loginRequest.Mask, model.PasswordCharacters);
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
    
    private async Task<LoginRequest> ValidateLoginRequestModelAndThrowAsync(LoginRequestModel model)
    {
        const string message = "Something went wrong. Try again later.";
        
        if (model.PasswordCharacters.Length != 5)
            throw new ArgumentException(message);
        
        var loginRequest = await _loginRequestRepository.GetLoginRequestAsync(model.Key);

        if (loginRequest is null ||
            loginRequest.Consumed ||
            DateTime.UtcNow > loginRequest.ExpiresAt ||
            loginRequest.Email != model.Email)
        {
            throw new ArgumentException(message);
        }
        
        await _loginRequestRepository.ConsumeLoginRequestAsync(loginRequest);

        return loginRequest;
    }

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
    
    private static List<(int pos, char c)> ParseMaskAndCharacters(string mask, string passwordCharacters)
    {
        return mask.Select((x, i) => (x, i))
            .Where(x => x.x == '1')
            .Select((x, i) => (x.i, passwordCharacters[i]))
            .ToList();
    }
    
    # endregion
}