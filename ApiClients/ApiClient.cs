using System.Net.Http.Json;
using GadgetShop.Models;
using Microsoft.Extensions.Logging;

namespace GadgetShop.ApiClients;

public interface IApiClient
{
    Task<ApiResponse<TResponse>> GetAsync<TResponse>(string url, CancellationToken ct = default);
    Task<ApiResponse<TResponse>> PostAsync<TRequest, TResponse>(string url, TRequest request, CancellationToken ct = default);
    Task<ApiResponse<TResponse>> PostEmptyAsync<TResponse>(string url, CancellationToken ct = default);
    Task<ApiResponse<TResponse>> PutAsync<TRequest, TResponse>(string url, TRequest request, CancellationToken ct = default);
    Task<ApiResponse<TResponse>> DeleteAsync<TResponse>(string url, CancellationToken ct = default);
}

public sealed class ApiClient(HttpClient http, ILogger<ApiClient> logger) : IApiClient
{
    public Task<ApiResponse<TResponse>> GetAsync<TResponse>(string url, CancellationToken ct = default)
        => SendAsync<TResponse>(() => http.GetAsync(url, ct));

    public Task<ApiResponse<TResponse>> PostAsync<TRequest, TResponse>(string url, TRequest request, CancellationToken ct = default)
        => SendAsync<TResponse>(() => http.PostAsJsonAsync(url, request, ct));

    public Task<ApiResponse<TResponse>> PostEmptyAsync<TResponse>(string url, CancellationToken ct = default)
        => SendAsync<TResponse>(() => http.PostAsync(url, null, ct));

    public Task<ApiResponse<TResponse>> PutAsync<TRequest, TResponse>(string url, TRequest request, CancellationToken ct = default)
        => SendAsync<TResponse>(() => http.PutAsJsonAsync(url, request, ct));

    public Task<ApiResponse<TResponse>> DeleteAsync<TResponse>(string url, CancellationToken ct = default)
        => SendAsync<TResponse>(() => http.DeleteAsync(url, ct));

    private async Task<ApiResponse<T>> SendAsync<T>(Func<Task<HttpResponseMessage>> call)
    {
        try
        {
            using var response = await call();
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return Fail<T>(401, "Session expired. Please log in again.");
            if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                return Fail<T>(403, "You do not have permission to perform this action.");

            var payload = await response.Content.ReadFromJsonAsync<ApiResponse<T>>();
            if (payload is not null) return payload;

            return new ApiResponse<T>
            {
                Code = (int)response.StatusCode,
                Message = response.ReasonPhrase,
                Errors = response.IsSuccessStatusCode ? new() : new() { response.ReasonPhrase ?? "Request failed" }
            };
        }
        catch (TaskCanceledException ex)
        {
            logger.LogWarning(ex, "API timeout {Url}", call.Method.Name);
            return Fail<T>(408, "Request timed out. Please try again.");
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, "API network error");
            return Fail<T>(503, "Cannot reach the server. Please check your connection.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected API error");
            return Fail<T>(500, "An unexpected error occurred.");
        }
    }

    private static ApiResponse<T> Fail<T>(int code, string message) =>
        new() { Code = code, Message = message, Errors = new() { message } };
}
