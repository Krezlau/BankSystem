using BankSystem.Data;
using BankSystem.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankSystem.Repositories.Users;

public interface IUserRepository
{
    Task<bool> CreateUserAsync(User user, List<PasswordKey> keys);

    Task<User?> GetUserWithPasswordAsync(string email);
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

    public async Task<User?> GetUserWithPasswordAsync(string email)
    {
        return await _dbContext.Users
            .Where(x => x.Email == email)
            .Include(x => x.PasswordKeys)
            .FirstOrDefaultAsync();

    }
}