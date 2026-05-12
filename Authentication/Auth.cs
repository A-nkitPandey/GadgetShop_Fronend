// ============================================================
// GadgetShop.Authentication
// ============================================================
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace GadgetShop.Authentication;

// ─── Token Storage ────────────────────────────────────────────
public sealed class TokenStorageService(ILocalStorageService storage)
{
    private const string TokenKey = "gs_access_token";
    private const string UserKey = "gs_user";

    public ValueTask<string?> GetAccessTokenAsync() =>
        storage.GetItemAsStringAsync(TokenKey);

    public ValueTask SetAccessTokenAsync(string token) =>
        storage.SetItemAsStringAsync(TokenKey, token);

    public ValueTask<T?> GetUserAsync<T>() =>
        storage.GetItemAsync<T>(UserKey);

    public ValueTask SetUserAsync<T>(T user) =>
        storage.SetItemAsync(UserKey, user);

    public async ValueTask ClearAsync()
    {
        await storage.RemoveItemAsync(TokenKey);
        await storage.RemoveItemAsync(UserKey);
    }
}

// ─── Auth State Provider ──────────────────────────────────────
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

    public void NotifyAuthChanged() =>
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());

    public async Task<List<string>> GetRolesAsync()
    {
        var state = await GetAuthenticationStateAsync();
        return state.User.Claims
            .Where(c => c.Type == ClaimTypes.Role || c.Type == "role")
            .Select(c => c.Value)
            .ToList();
    }

    public async Task<string?> GetUsernameAsync()
    {
        var state = await GetAuthenticationStateAsync();
        return state.User.Identity?.Name
               ?? state.User.FindFirst("name")?.Value
               ?? state.User.FindFirst(ClaimTypes.Name)?.Value;
    }

    private static AuthenticationState Anonymous() =>
        new(new ClaimsPrincipal(new ClaimsIdentity()));

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
        catch { return true; }
    }
}

// ─── Auth Header Handler ──────────────────────────────────────
public sealed class AuthHeaderHandler(TokenStorageService storage) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken ct)
    {
        var token = await storage.GetAccessTokenAsync();
        if (!string.IsNullOrWhiteSpace(token))
            request.Headers.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        return await base.SendAsync(request, ct);
    }
}
