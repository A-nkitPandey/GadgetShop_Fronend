using Blazored.LocalStorage;
using GadgetShop.Constants;

namespace GadgetShop.Authentication;

public sealed class TokenStorageService(ILocalStorageService storage)
{
    public ValueTask<string?> GetAccessTokenAsync() => storage.GetItemAsStringAsync(StorageKeys.AccessToken);
    public ValueTask SetAccessTokenAsync(string token) => storage.SetItemAsStringAsync(StorageKeys.AccessToken, token);
    public ValueTask<string?> GetRefreshTokenAsync() => storage.GetItemAsStringAsync(StorageKeys.RefreshToken);
    public ValueTask SetRefreshTokenAsync(string token) => storage.SetItemAsStringAsync(StorageKeys.RefreshToken, token);
    public ValueTask<T?> GetUserAsync<T>() => storage.GetItemAsync<T>(StorageKeys.User);
    public ValueTask SetUserAsync<T>(T user) => storage.SetItemAsync(StorageKeys.User, user);

    public async ValueTask ClearAsync()
    {
        await storage.RemoveItemAsync(StorageKeys.AccessToken);
        await storage.RemoveItemAsync(StorageKeys.RefreshToken);
        await storage.RemoveItemAsync(StorageKeys.User);
    }
}
