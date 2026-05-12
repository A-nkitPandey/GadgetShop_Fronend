using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace GadgetShop.Authentication;

public sealed class AuthStateProvider(TokenStorageService storage) : AuthenticationStateProvider
{
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await storage.GetAccessTokenAsync();
        if (string.IsNullOrWhiteSpace(token) || IsTokenExpired(token))
            return Anonymous();

        var identity = new ClaimsIdentity(ParseClaims(token), "jwt");
        return new AuthenticationState(new ClaimsPrincipal(identity));
    }

    public void NotifyAuthChanged() => NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());

    public async Task<List<string>> GetRolesAsync()
    {
        var state = await GetAuthenticationStateAsync();
        return state.User.Claims
            .Where(c => c.Type == ClaimTypes.Role || c.Type == "role")
            .Select(c => c.Value)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    public async Task<string?> GetUsernameAsync()
    {
        var state = await GetAuthenticationStateAsync();
        return state.User.Identity?.Name
               ?? state.User.FindFirst("name")?.Value
               ?? state.User.FindFirst(ClaimTypes.Name)?.Value;
    }

    private static AuthenticationState Anonymous() => new(new ClaimsPrincipal(new ClaimsIdentity()));

    private static IEnumerable<Claim> ParseClaims(string jwt)
    {
        try { return new JwtSecurityTokenHandler().ReadJwtToken(jwt).Claims; }
        catch { return Enumerable.Empty<Claim>(); }
    }

    private static bool IsTokenExpired(string jwt)
    {
        try
        {
            var token = new JwtSecurityTokenHandler().ReadJwtToken(jwt);
            return token.ValidTo < DateTime.UtcNow;
        }
        catch
        {
            return true;
        }
    }
}
