using GadgetShop.Models;

namespace GadgetShop.ApiClients;

public interface IApiClient
{
    Task<ApiResponse<TResponse>> GetAsync<TResponse>(string url, CancellationToken ct = default);
    Task<ApiResponse<TResponse>> PostAsync<TRequest, TResponse>(string url, TRequest request, CancellationToken ct = default);
    Task<ApiResponse<TResponse>> PostEmptyAsync<TResponse>(string url, CancellationToken ct = default);
    Task<ApiResponse<TResponse>> PutAsync<TRequest, TResponse>(string url, TRequest request, CancellationToken ct = default);
    Task<ApiResponse<TResponse>> DeleteAsync<TResponse>(string url, CancellationToken ct = default);
}
