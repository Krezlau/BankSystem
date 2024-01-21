using BankSystem.Data;
using BankSystem.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankSystem.Repositories.Users;

public interface IUserRepository
{
    Task<bool> CreateUserAsync(User user, List<PasswordKey> keys);

    Task UpdateUserAsync(User user);

    Task<User?> GetUserWithPasswordAsync(string email);
    
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

    public async Task<User?> GetUserWithPasswordAsync(string email)
    {
        return await _dbContext.Users
            .Where(x => x.Email == email)
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