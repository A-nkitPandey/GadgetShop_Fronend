using GadgetShop.Models;

namespace GadgetShop.State;

// ─── Cart State ───────────────────────────────────────────────
public sealed class CartState
{
    private CartDto? _cart;

    public CartDto? Cart => _cart;
    public int ItemCount => _cart?.Items.Sum(i => i.Quantity) ?? 0;
    public decimal Total => _cart?.TotalAmount ?? 0;

    public event Action? OnChange;

    public void SetCart(CartDto? cart)
    {
        _cart = cart;
        OnChange?.Invoke();
    }

    public void Clear()
    {
        _cart = null;
        OnChange?.Invoke();
    }
}

// ─── Auth State ───────────────────────────────────────────────
public sealed class AuthState
{
    public string? UserName { get; private set; }
    public List<string> Roles { get; private set; } = new();
    public bool IsAuthenticated => !string.IsNullOrWhiteSpace(UserName);
    public bool IsAdmin => Roles.Contains("Admin") || Roles.Contains("SuperAdmin");

    public event Action? OnChange;

    public void SetUser(string? userName, List<string> roles)
    {
        UserName = userName;
        Roles = roles;
        OnChange?.Invoke();
    }

    public void Clear()
    {
        UserName = null;
        Roles = new();
        OnChange?.Invoke();
    }
}
