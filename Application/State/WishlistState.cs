namespace GadgetShop.State;

public sealed class WishlistState
{
    public int ItemCount { get; private set; }
    public event Action? OnChange;

    public void SetCount(int itemCount)
    {
        ItemCount = Math.Max(0, itemCount);
        OnChange?.Invoke();
    }

    public void Clear()
    {
        ItemCount = 0;
        OnChange?.Invoke();
    }
}
