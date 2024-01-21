using System.Text;
using BankSystem.Data.Entities;
using BankSystem.Data.Mapping;
using BankSystem.Data.Models;
using BankSystem.Repositories.Auth;
using BankSystem.Repositories.BankAccounts;
using BankSystem.Repositories.Users;

namespace BankSystem.Services.Auth;

public interface IAuthService
{
    Task<LoginCheckResponseModel> LoginCheckAsync(LoginCheckRequestModel model);
    
    Task<AuthResponse> LoginAsync(LoginRequestModel model);
    
    Task<bool> RegisterAsync(RegisterRequestModel model);
    
    Task<bool> ChangePasswordAsync(ChangePasswordRequestModel model, Guid userId);
}


public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly ILoginRequestRepository _loginRequestRepository;
    private readonly IJwtService _jwtService;
    private readonly IBankAccountRepository _bankAccountRepository;

    public AuthService(IUserRepository userRepository,
        ILoginRequestRepository loginRequestRepository,
        IJwtService jwtService,
        IBankAccountRepository bankAccountRepository)
    {
        _userRepository = userRepository;
        _loginRequestRepository = loginRequestRepository;
        _jwtService = jwtService;
        _bankAccountRepository = bankAccountRepository;
    }

    public async Task<LoginCheckResponseModel> LoginCheckAsync(LoginCheckRequestModel model)
    {
        // check if there's a valid login request
        var loginRequest = await _loginRequestRepository.GetValidLoginRequestForUserAsync(model.Email);
        if (loginRequest is not null) return new LoginCheckResponseModel(loginRequest.Mask, loginRequest.Id);
        
        //var mask = GenerateMask();
        var mask = "010101010100000000000000"; // for testing
        loginRequest = await _loginRequestRepository.CreateLoginRequestAsync(mask, model.Email);
        
        return new LoginCheckResponseModel(mask, loginRequest.Id);
    }

    public async Task<AuthResponse> LoginAsync(LoginRequestModel model)
    {
        var loginRequest = await ValidateLoginRequestModelAndThrowAsync(model.PasswordCharacters, model.Email, model.Key);
        
        var user = await _userRepository.GetUserWithPasswordAsync(model.Email);
        if (user is null)
            return await FailRequestAsync(user, model.Email, loginRequest);
        
        if (user.IsLocked && user.LockedUntil > DateTime.UtcNow)
            return AuthResponseHelper.Locked();
        
        if (user.IsLocked) // unlock user if locked time has passed
        {
            user.IsLocked = false;
            user.LockedUntil = DateTime.UtcNow;
            await _userRepository.UpdateUserAsync(user);
        }
        
        var passwordChars = ParseMaskAndCharacters(loginRequest.Mask, model.PasswordCharacters);
        if (!PasswordService.VerifyPassword(passwordChars, user.SecretHash, user.PasswordKeys))
            return await FailRequestAsync(user, model.Email, loginRequest);
            
        var token = _jwtService.GenerateJwtToken(user.Email, user.Id);
        return AuthResponseHelper.Success(token);
    }

    public async Task<bool> RegisterAsync(RegisterRequestModel model)
    {
        var partialPassword = PasswordService.CreatePassword(model.Password);
        var user = model.ToEntity(partialPassword);
        await _userRepository.CreateUserAsync(user, partialPassword.keys);
        await _bankAccountRepository.AssignCardNumberAsync(user.BankAccount);
        await _bankAccountRepository.GiveUserOneHundredPLNAsync(user.BankAccount);
        return true;
    }

    public async Task<bool> ChangePasswordAsync(ChangePasswordRequestModel model, Guid userId)
    {
        var user = await _userRepository.GetUserWithPasswordAsync(userId);
        if (user is null) return false;
        
        var loginRequest = await ValidateLoginRequestModelAndThrowAsync(model.PasswordCharacters, user.Email, model.Key);
        
        var passwordChars = ParseMaskAndCharacters(loginRequest.Mask, model.PasswordCharacters);
        if (!PasswordService.VerifyPassword(passwordChars, user.SecretHash, user.PasswordKeys))
            return (await FailRequestAsync(user, user.Email, loginRequest)).Success;
        
        var newPassword = PasswordService.CreatePassword(model.NewPassword);
        await _userRepository.UpdateUserPasswordAsync(user, newPassword.hashedSecret, newPassword.keys);
        return true;
    }

    # region private methods
    
    private async Task<LoginRequest> ValidateLoginRequestModelAndThrowAsync(string passwordCharacters, string email, Guid key)
    {
        const string message = "Something went wrong. Try again later.";
        
        if (passwordCharacters.Length != 5)
            throw new ArgumentException(message);
        
        var loginRequest = await _loginRequestRepository.GetLoginRequestAsync(key);

        if (loginRequest is null ||
            loginRequest.Consumed ||
            DateTime.UtcNow > loginRequest.ExpiresAt ||
            loginRequest.Email != email)
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

    private async Task<AuthResponse> FailRequestAsync(User? user, string email, LoginRequest loginRequest)
    {
        await _loginRequestRepository.FailLoginRequestAsync(loginRequest);
        
        var failedCount = await _loginRequestRepository.CountFailedLoginRequestsForUserInOneHourTimeWindowAsync(email);

        if (failedCount < 3 || user is null) return AuthResponseHelper.Failed(failedCount);
        
        user.IsLocked = true;
        user.LockedUntil = DateTime.UtcNow.AddHours(1);
        await _userRepository.UpdateUserAsync(user);

        return AuthResponseHelper.Failed(failedCount);
    }
    
    # endregion
}