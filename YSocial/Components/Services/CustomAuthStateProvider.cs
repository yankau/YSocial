using System.Security.Claims;
using System.Threading.Tasks;
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
    private readonly AppDbContext _dbContext;

    public CustomAuthStateProvider(IHttpContextAccessor httpContextAccessor, AppDbContext dbContext)
    {
        _httpContextAccessor = httpContextAccessor;
        _dbContext = dbContext;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext?.User?.Identity?.IsAuthenticated == true)
        {
            var username = httpContext.User.Identity.Name;
            var user = await _dbContext.UserAccounts
                .FirstOrDefaultAsync(u => u.Username == username);

            if (user != null)
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, user.Username ?? string.Empty),
                    new Claim("AvatarUrl", user.AvatarUrl ?? "/images/default-avatar.png"),
                    new Claim("Description", user.Description ?? string.Empty)
                };
                var identity = new ClaimsIdentity(claims, httpContext.User.Identity.AuthenticationType);
                var principal = new ClaimsPrincipal(identity);
                return new AuthenticationState(principal);
            }
        }

        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
    }

    public async Task SignInAsync(string username)
    {
        var user = await _dbContext.UserAccounts
            .FirstOrDefaultAsync(u => u.Username == username);

        if (user != null && _httpContextAccessor.HttpContext != null)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username ?? string.Empty),
                new Claim("AvatarUrl", user.AvatarUrl ?? "/images/default-avatar.png"),
                new Claim("Description", user.Description ?? string.Empty)
            };
            var identity = new ClaimsIdentity(claims, "cookie");
            var principal = new ClaimsPrincipal(identity);
            await _httpContextAccessor.HttpContext.SignInAsync("cookie", principal);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }

    public async Task SignOutAsync()
    {
        if (_httpContextAccessor.HttpContext != null)
        {
            await _httpContextAccessor.HttpContext.SignOutAsync("cookie");
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}