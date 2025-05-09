using Microsoft.EntityFrameworkCore;
using YSocial.Components.Models;

namespace YSocial.Components.Services;

public class AuthService
{
    private readonly AppDbContext _dbContext;

    public AuthService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> LoginAsync(string username, string password)
    {
        var user = await _dbContext.UserAccounts
            .FirstOrDefaultAsync(u => u.Username == username);

        if (user == null || user.Password != Cryptography.Sha256Hash(password))
            return false;

        return true;
    }
}