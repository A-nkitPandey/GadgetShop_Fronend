using System.Net.Http.Json;
using GadgetShop.Constants;
using GadgetShop.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using MudBlazor;

namespace GadgetShop.Authentication;

public sealed class TokenRefreshCoordinator(
    IHttpClientFactory httpClientFactory,
    TokenStorageService storage,
    AuthStateProvider authStateProvider,
    NavigationManager navigationManager,
    ISnackbar snackbar,
    ILogger<TokenRefreshCoordinator> logger)
{
    private readonly SemaphoreSlim _refreshLock = new(1, 1);
    private bool _sessionExpiredShown;

    public async Task<(bool Success, string? AccessToken)> TryRefreshAsync(string? previousAccessToken, CancellationToken ct = default)
    {
        await _refreshLock.WaitAsync(ct);
        try
        {
            var currentAccessToken = await storage.GetAccessTokenAsync();
            if (!string.IsNullOrWhiteSpace(currentAccessToken) &&
                !string.Equals(currentAccessToken, previousAccessToken, StringComparison.Ordinal))
            {
                return (true, currentAccessToken);
            }

            var refreshToken = await storage.GetRefreshTokenAsync();
            if (string.IsNullOrWhiteSpace(currentAccessToken) || string.IsNullOrWhiteSpace(refreshToken))
            {
                await HandleRefreshFailureAsync();
                return (false, null);
            }

            var client = httpClientFactory.CreateClient("GadgetApiNoAuth");
            using var response = await client.PostAsJsonAsync(
                ApiEndpoints.Auth.RefreshToken,
                new RefreshTokenRequest
                {
                    AccessToken = currentAccessToken,
                    RefreshToken = refreshToken
                },
                ct);

            var payload = await response.Content.ReadFromJsonAsync<ApiResponse<AuthTokenResponse>>(cancellationToken: ct);
            if (response.IsSuccessStatusCode &&
                payload?.IsSuccess == true &&
                !string.IsNullOrWhiteSpace(payload.Data?.AccessToken))
            {
                await storage.SetAccessTokenAsync(payload.Data.AccessToken);
                await storage.SetRefreshTokenAsync(payload.Data.RefreshToken);

                var user = await storage.GetUserAsync<LoginResponse>();
                if (user != null)
                {
                    user.AccessToken = payload.Data.AccessToken;
                    user.RefreshToken = payload.Data.RefreshToken;
                    user.ExpiresAtUtc = payload.Data.ExpiresAtUtc;
                    user.RefreshTokenExpiryTime = payload.Data.RefreshTokenExpiryTime;
                    await storage.SetUserAsync(user);
                }

                _sessionExpiredShown = false;
                authStateProvider.NotifyAuthChanged();
                return (true, payload.Data.AccessToken);
            }

            await HandleRefreshFailureAsync();
            return (false, null);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Refresh token flow failed.");
            await HandleRefreshFailureAsync();
            return (false, null);
        }
        finally
        {
            _refreshLock.Release();
        }
    }

    private async Task HandleRefreshFailureAsync()
    {
        await storage.ClearAsync();
        authStateProvider.NotifyAuthChanged();

        if (!_sessionExpiredShown)
        {
            snackbar.Add("Session expired. Please log in again.", Severity.Warning);
            _sessionExpiredShown = true;
        }

        navigationManager.NavigateTo("/login");
    }
}
