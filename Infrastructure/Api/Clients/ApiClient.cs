using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using GadgetShop.Models;
using Microsoft.Extensions.Logging;

namespace GadgetShop.ApiClients;

public sealed class ApiClient(HttpClient http, ILogger<ApiClient> logger) : IApiClient
{
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

    public Task<ApiResponse<TResponse>> GetAsync<TResponse>(string url, CancellationToken ct = default)
        => SendAsync<TResponse>(url, () => http.GetAsync(url, ct));

    public Task<ApiResponse<TResponse>> PostAsync<TRequest, TResponse>(string url, TRequest request, CancellationToken ct = default)
        => SendAsync<TResponse>(url, () => http.PostAsJsonAsync(url, request, ct));

    public Task<ApiResponse<TResponse>> PostMultipartAsync<TResponse>(string url, MultipartFormDataContent content, CancellationToken ct = default)
        => SendAsync<TResponse>(url, () => http.PostAsync(url, content, ct));

    public Task<ApiResponse<TResponse>> PostEmptyAsync<TResponse>(string url, CancellationToken ct = default)
        => SendAsync<TResponse>(url, () => http.PostAsync(url, null, ct));

    public Task<ApiResponse<TResponse>> PutAsync<TRequest, TResponse>(string url, TRequest request, CancellationToken ct = default)
        => SendAsync<TResponse>(url, () => http.PutAsJsonAsync(url, request, ct));

    public Task<ApiResponse<TResponse>> DeleteAsync<TResponse>(string url, CancellationToken ct = default)
        => SendAsync<TResponse>(url, () => http.DeleteAsync(url, ct));

    private async Task<ApiResponse<T>> SendAsync<T>(string url, Func<Task<HttpResponseMessage>> call)
    {
        try
        {
            using var response = await call();

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                return Fail<T>(401, "Session expired. Please log in again.");

            if (response.StatusCode == HttpStatusCode.Forbidden)
                return Fail<T>(403, "You do not have permission to perform this action.");

            var content = response.Content is null ? null : await response.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(content))
            {
                return new ApiResponse<T>
                {
                    Code = (int)response.StatusCode,
                    Message = response.IsSuccessStatusCode ? "Request completed." : response.ReasonPhrase,
                    Errors = response.IsSuccessStatusCode ? new() : new() { response.ReasonPhrase ?? "Request failed" }
                };
            }

            var wrapped = JsonSerializer.Deserialize<ApiResponse<T>>(content, JsonOptions);
            if (wrapped is not null)
                return wrapped;

            if (response.IsSuccessStatusCode)
            {
                var data = JsonSerializer.Deserialize<T>(content, JsonOptions);
                return new ApiResponse<T> { Code = (int)response.StatusCode, Message = "Request completed.", Data = data };
            }

            logger.LogWarning("Unexpected API payload from {Url}: {Payload}", url, content);
            return Fail<T>((int)response.StatusCode, response.ReasonPhrase ?? "Request failed");
        }
        catch (TaskCanceledException ex)
        {
            logger.LogWarning(ex, "API timeout calling {Url}", url);
            return Fail<T>(408, "The server took too long to respond. Please try again.");
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, "Network error calling {Url}", url);
            return Fail<T>(503, "Cannot reach the server right now. Please try again shortly.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected API error calling {Url}", url);
            return Fail<T>(500, "Something went wrong while talking to the server.");
        }
    }

    private static ApiResponse<T> Fail<T>(int code, string message) =>
        new() { Code = code, Message = message, Errors = new() { message } };
}
