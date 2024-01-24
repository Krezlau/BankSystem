using BankSystem.Data;
using BankSystem.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankSystem.Repositories.Users;

public interface IUserRepository
{
    Task<bool> CreateUserAsync(User user, List<PasswordKey> keys);

    Task UpdateUserAsync(User user);
    
    Task UpdateUserPasswordAsync(User user, string hashedSecret, List<PasswordKey> keys);
    
    Task<User?> GetUserAsync(Guid userId);

    Task<User?> GetUserWithPasswordAsync(string email);
    
    Task<User?> GetUserWithPasswordAsync(Guid userId);
    
    Task<UserSensitiveData?> GetUserSensitiveDataAsync(Guid userId);
}

public class UserRepository : IUserRepository
{
    private readonly BankDbContext _dbContext;

    public UserRepository(BankDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> CreateUserAsync(User user, List<PasswordKey> keys)
    {
        _dbContext.Users.Add(user);
        keys.ForEach(x => x.User = user);
        await _dbContext.AddRangeAsync(keys);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task UpdateUserAsync(User user)
    {
        _dbContext.Users.Update(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateUserPasswordAsync(User user, string hashedSecret, List<PasswordKey> keys)
    {
        //_dbContext.PasswordKeys.RemoveRange(user.PasswordKeys);
        // await _dbContext.PasswordKeys.AddRangeAsync(keys);
        user.SecretHash = hashedSecret;
        user.PasswordKeys = keys;
        _dbContext.Users.Update(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<User?> GetUserAsync(Guid userId)
    {
        return await _dbContext.Users
            .Where(x => x.Id == userId)
            .FirstOrDefaultAsync();
    }

    public async Task<User?> GetUserWithPasswordAsync(string email)
    {
        return await _dbContext.Users
            .Where(x => x.Email == email)
            .Include(x => x.PasswordKeys.OrderBy(x => x.CreatedAt))
            .FirstOrDefaultAsync();
    }

    public async Task<User?> GetUserWithPasswordAsync(Guid userId)
    {
        return await _dbContext.Users
            .Where(x => x.Id == userId)
            .Include(x => x.PasswordKeys)
            .FirstOrDefaultAsync();
    }

    public async Task<UserSensitiveData?> GetUserSensitiveDataAsync(Guid userId)
    {
        return await _dbContext.UserSensitiveData
            .Where(x => x.UserId == userId)
            .FirstOrDefaultAsync();
    }
}