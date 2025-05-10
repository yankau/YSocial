using Microsoft.EntityFrameworkCore;
using YSocial.Components.Models;

public class AuthService
{
    private readonly IDbContextFactory<AppDbContext> _dbContextFactory;

    public AuthService(IDbContextFactory<AppDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task<bool> LoginAsync(string username, string password)
    {
        using var dbContext = _dbContextFactory.CreateDbContext();
        var user = await dbContext.UserAccounts
            .FirstOrDefaultAsync(u => u.Username == username);

        if (user == null || user.Password != Cryptography.Sha256Hash(password))
            return false;

        // Update online status
        user.IsOnline = true;
        user.LastSeen = DateTime.UtcNow;
        await dbContext.SaveChangesAsync();

        return true;
    }

    public async Task UpdateLastSeenAsync(string username)
    {
        using var dbContext = _dbContextFactory.CreateDbContext();
        var user = await dbContext.UserAccounts
            .FirstOrDefaultAsync(u => u.Username == username);

        if (user != null)
        {
            user.LastSeen = DateTime.UtcNow;
            user.IsOnline = true;
            await dbContext.SaveChangesAsync();
        }
    }

    public async Task LogoutAsync(string username)
    {
        using var dbContext = _dbContextFactory.CreateDbContext();
        var user = await dbContext.UserAccounts
            .FirstOrDefaultAsync(u => u.Username == username);

        if (user != null)
        {
            user.IsOnline = false;
            await dbContext.SaveChangesAsync();
        }
    }
}