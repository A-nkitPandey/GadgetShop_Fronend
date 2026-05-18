using GadgetShop.Models;

namespace GadgetShop.State;

public sealed class CartState
{
    private CartDto? _cart;

    public CartDto? Cart => _cart;
    public int ItemCount => _cart?.Items.Sum(i => i.Quantity) ?? 0;
    public decimal Total => _cart?.TotalAmount ?? 0;
    public bool IsInitialized { get; private set; }

    public event Action? OnChange;

    public void SetCart(CartDto? cart)
    {
        _cart = cart;
        IsInitialized = true;
        OnChange?.Invoke();
    }

    public void Clear()
    {
        _cart = null;
        IsInitialized = true;
        OnChange?.Invoke();
    }

    public void Reset()
    {
        _cart = null;
        IsInitialized = false;
        OnChange?.Invoke();
    }
}
