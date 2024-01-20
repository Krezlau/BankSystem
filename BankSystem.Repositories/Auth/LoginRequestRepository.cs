using BankSystem.Data;
using BankSystem.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankSystem.Repositories.Auth;

public interface ILoginRequestRepository
{
    Task<LoginRequest?> GetLoginRequestAsync(Guid id);
    
    Task<LoginRequest?> GetValidLoginRequestForUserAsync(string email);
    
    Task<int> CountFailedLoginRequestsForUserInOneHourTimeWindowAsync(string email);
    
    Task<LoginRequest> CreateLoginRequestAsync(string mask, string email);

    Task ConsumeLoginRequestAsync(LoginRequest loginRequest);
    
    Task FailLoginRequestAsync(LoginRequest loginRequest);
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

    public async Task<LoginRequest?> GetValidLoginRequestForUserAsync(string email)
    {
        var loginRequest = await _dbContext.LoginRequests
            .Where(x => x.Email == email && !x.Consumed && x.ExpiresAt > DateTime.UtcNow)
            .FirstOrDefaultAsync();
        
        if (loginRequest is null) return null;
        
        loginRequest.ExpiresAt = DateTime.UtcNow.AddMinutes(5);
        _dbContext.LoginRequests.Update(loginRequest);
        await _dbContext.SaveChangesAsync();
        
        return loginRequest;
    }

    public async Task<int> CountFailedLoginRequestsForUserInOneHourTimeWindowAsync(string email)
    {
        var oneHourAgo = DateTime.UtcNow.AddHours(-1);
        return await _dbContext.LoginRequests
            .Where(x => x.Email == email && x.Failed && x.CreatedAt > oneHourAgo)
            .CountAsync();
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

    public async Task FailLoginRequestAsync(LoginRequest loginRequest)
    {
        loginRequest.Failed = true;
        _dbContext.LoginRequests.Update(loginRequest);
        await _dbContext.SaveChangesAsync();
    }
}