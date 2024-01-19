using BankSystem.Data;
using BankSystem.Data.Entities;

namespace BankSystem.Repositories.Auth;

public interface ILoginRequestRepository
{
    Task<LoginRequest?> GetLoginRequestAsync(Guid id);
    
    Task<LoginRequest> CreateLoginRequestAsync(string mask, string email);

    Task ConsumeLoginRequestAsync(LoginRequest loginRequest);
}

public class LoginRequestRepository : ILoginRequestRepository
{
    private readonly BankDbContext _dbContext;

    public LoginRequestRepository(BankDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<LoginRequest?> GetLoginRequestAsync(Guid id)
    {
        return await _dbContext.LoginRequests.FindAsync(id);
    }

    public async Task<LoginRequest> CreateLoginRequestAsync(string mask, string email)
    {
        var loginRequest = new LoginRequest
        {
            Mask = mask,
            Email = email,
            Consumed = false,
            ExpiresAt = DateTime.UtcNow.AddMinutes(10)
        };
        await _dbContext.LoginRequests.AddAsync(loginRequest);
        await _dbContext.SaveChangesAsync();
        return loginRequest;
    }

    public async Task ConsumeLoginRequestAsync(LoginRequest loginRequest)
    {
        loginRequest.Consumed = true;
        _dbContext.LoginRequests.Update(loginRequest);
        await _dbContext.SaveChangesAsync();
    }
}