using System.Net;
using System.Net.Http.Headers;
using GadgetShop.Constants;

namespace GadgetShop.Authentication;

public sealed class AuthHeaderHandler(TokenStorageService storage, TokenRefreshCoordinator refreshCoordinator) : DelegatingHandler
{
    private static readonly HttpRequestOptionsKey<bool> RetryKey = new("gs-auth-retried");

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct)
    {
        var token = await storage.GetAccessTokenAsync();
        if (!string.IsNullOrWhiteSpace(token))
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await base.SendAsync(request, ct);
        if (response.StatusCode != HttpStatusCode.Unauthorized ||
            request.RequestUri?.AbsolutePath.Contains(ApiEndpoints.Auth.RefreshToken, StringComparison.OrdinalIgnoreCase) == true ||
            request.Options.TryGetValue(RetryKey, out var alreadyRetried) && alreadyRetried)
        {
            return response;
        }

        var refreshResult = await refreshCoordinator.TryRefreshAsync(token, ct);
        if (!refreshResult.Success || string.IsNullOrWhiteSpace(refreshResult.AccessToken))
            return response;

        response.Dispose();

        var retryRequest = await CloneRequestAsync(request, ct);
        retryRequest.Options.Set(RetryKey, true);
        retryRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", refreshResult.AccessToken);
        return await base.SendAsync(retryRequest, ct);
    }

    private static async Task<HttpRequestMessage> CloneRequestAsync(HttpRequestMessage request, CancellationToken ct)
    {
        var clone = new HttpRequestMessage(request.Method, request.RequestUri)
        {
            Version = request.Version,
            VersionPolicy = request.VersionPolicy
        };

        foreach (var header in request.Headers)
            clone.Headers.TryAddWithoutValidation(header.Key, header.Value);

        foreach (var option in request.Options)
            clone.Options.Set(new HttpRequestOptionsKey<object?>(option.Key), option.Value);

        if (request.Content != null)
        {
            var bytes = await request.Content.ReadAsByteArrayAsync(ct);
            var content = new ByteArrayContent(bytes);

            foreach (var header in request.Content.Headers)
                content.Headers.TryAddWithoutValidation(header.Key, header.Value);

            clone.Content = content;
        }

        return clone;
    }
}
