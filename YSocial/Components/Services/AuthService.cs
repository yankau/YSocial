using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using YSocial.Components.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

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

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly ProtectedLocalStorage _localStorage;
    private readonly AppDbContext _dbContext;
    private ClaimsPrincipal _currentUser = new ClaimsPrincipal(new ClaimsIdentity());

    public CustomAuthStateProvider(ProtectedLocalStorage localStorage, AppDbContext dbContext)
    {
        _localStorage = localStorage;
        _dbContext = dbContext;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var storedUsername = await _localStorage.GetAsync<string>("username");
            if (storedUsername.Success && !string.IsNullOrEmpty(storedUsername.Value))
            {
                var user = await _dbContext.UserAccounts
                    .FirstOrDefaultAsync(u => u.Username == storedUsername.Value);
                
                if (user != null)
                {
                    var identity = new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.Name, user.Username),
                    }, "auth");
                    
                    _currentUser = new ClaimsPrincipal(identity);
                }
            }
        }
        catch
        {
            // Если произошла ошибка при чтении из localStorage, считаем пользователя неаутентифицированным
            _currentUser = new ClaimsPrincipal(new ClaimsIdentity());
        }
        
        return new AuthenticationState(_currentUser);
    }

    public async Task SetAuthenticatedUser(string username)
    {
        var identity = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Name, username),
        }, "auth");
        
        _currentUser = new ClaimsPrincipal(identity);
        await _localStorage.SetAsync("username", username);
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public async Task SetUnauthenticatedUser()
    {
        _currentUser = new ClaimsPrincipal(new ClaimsIdentity());
        await _localStorage.DeleteAsync("username");
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}

