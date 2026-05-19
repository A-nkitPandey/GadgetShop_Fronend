using GadgetShop.Constants;

namespace GadgetShop.State;

public sealed class AuthState
{
    public string? UserName { get; private set; }
    public List<string> Roles { get; private set; } = new();
    public bool IsAuthenticated => !string.IsNullOrWhiteSpace(UserName);
    public bool IsAdmin => AppRoles.IsAdmin(Roles);
    public bool IsSuperAdmin => AppRoles.IsSuperAdmin(Roles);
    public bool IsCustomer => AppRoles.IsCustomer(Roles);
    public bool CanUseCart => AppRoles.CanUseCart(Roles);
    public bool CanUseWishlist => AppRoles.CanUseWishlist(Roles);

    public event Action? OnChange;

    public void SetUser(string? userName, IEnumerable<string>? roles)
    {
        UserName = userName;
        Roles = roles?.Distinct(StringComparer.OrdinalIgnoreCase).ToList() ?? new();
        OnChange?.Invoke();
    }

    public void Clear()
    {
        UserName = null;
        Roles = new();
        OnChange?.Invoke();
    }
}
