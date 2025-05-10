using Microsoft.EntityFrameworkCore;
using YSocial.Components.Models;

namespace YSocial.Components.Services;

public class AuthService
{
    private readonly IDbContextFactory<AppDbContext> _dbContextFactory;

    public AuthService(IDbContextFactory<AppDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task<bool> LoginAsync(string username, string password)
    {
        using var dbContext = _dbContextFactory.CreateDbContext(); // Fixed typo: _dbContext_factory -> _dbContextFactory
        var user = await dbContext.UserAccounts
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Username == username);

        if (user == null || user.Password != Cryptography.Sha256Hash(password))
            return false;

        return true;
    }
}