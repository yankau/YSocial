using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using YSocial.Components.Models;

namespace YSocial.Components.Services;
public class CustomAuthStateProvider : ServerAuthenticationStateProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IDbContextFactory<AppDbContext> _dbContextFactory;
    private readonly AuthService _authService;

    public CustomAuthStateProvider(IHttpContextAccessor httpContextAccessor, IDbContextFactory<AppDbContext> dbContextFactory, AuthService authService)
    {
        _httpContextAccessor = httpContextAccessor;
        _dbContextFactory = dbContextFactory;
        _authService = authService;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext?.User?.Identity?.IsAuthenticated != true || string.IsNullOrEmpty(httpContext.User.Identity.Name))
        {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        var username = httpContext.User.Identity.Name;
        using var dbContext = _dbContextFactory.CreateDbContext();
        var user = await dbContext.UserAccounts
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Username == username);

        if (user == null)
        {
            Console.WriteLine($"GetAuthenticationStateAsync: User {username} not found");
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        // Update last seen
        await _authService.UpdateLastSeenAsync(username);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Username ?? string.Empty),
            new Claim("AvatarUrl", user.AvatarUrl ?? "/images/default-avatar.png"),
            new Claim("Description", user.Description ?? string.Empty)
        };
        var identity = new ClaimsIdentity(claims, "cookie");
        var principal = new ClaimsPrincipal(identity);
        return new AuthenticationState(principal);
    }

    public async Task SignInAsync(string username)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null)
        {
            Console.WriteLine("SignInAsync: HttpContext is null");
            return;
        }

        using var dbContext = _dbContextFactory.CreateDbContext();
        var user = await dbContext.UserAccounts
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Username == username);

        if (user == null)
        {
            Console.WriteLine($"SignInAsync: User {username} not found");
            return;
        }

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Username ?? string.Empty),
            new Claim("AvatarUrl", user.AvatarUrl ?? "/images/default-avatar.png"),
            new Claim("Description", user.Description ?? string.Empty)
        };
        var identity = new ClaimsIdentity(claims, "cookie");
        var principal = new ClaimsPrincipal(identity);
        await httpContext.SignInAsync("cookie", principal, new AuthenticationProperties
        {
            IsPersistent = true,
            ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
        });
        Console.WriteLine($"SignInAsync: Signed in user {username}, notifying state change");
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public async Task SignOutAsync()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext != null)
        {
            var username = httpContext.User?.Identity?.Name;
            if (!string.IsNullOrEmpty(username))
            {
                await _authService.LogoutAsync(username);
            }

            await httpContext.SignOutAsync("cookie");
            httpContext.Response.Cookies.Delete("YSocialAuth");
            Console.WriteLine("SignOutAsync: User signed out, cookie cleared");
        }
        else
        {
            Console.WriteLine("SignOutAsync: HttpContext is null");
        }

        var emptyState = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        NotifyAuthenticationStateChanged(Task.FromResult(emptyState));
    }
}