using GadgetShop.Constants;

namespace GadgetShop.State;

public sealed class AuthState
{
    public string? UserName { get; private set; }
    public List<string> Roles { get; private set; } = new();
    public bool IsAuthenticated => !string.IsNullOrWhiteSpace(UserName);
    public bool IsAdmin => AppRoles.IsAdmin(Roles);

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
