namespace GadgetShop.State;

public sealed class WishlistState
{
    public int ItemCount { get; private set; }
    public bool IsInitialized { get; private set; }
    public event Action? OnChange;

    public void SetCount(int itemCount)
    {
        ItemCount = Math.Max(0, itemCount);
        IsInitialized = true;
        OnChange?.Invoke();
    }

    public void Clear()
    {
        ItemCount = 0;
        IsInitialized = true;
        OnChange?.Invoke();
    }

    public void Reset()
    {
        ItemCount = 0;
        IsInitialized = false;
        OnChange?.Invoke();
    }
}
