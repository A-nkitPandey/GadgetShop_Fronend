using GadgetShop.Models;

namespace GadgetShop.State;

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
