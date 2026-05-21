namespace GadgetShop.State;

public sealed class NotificationState
{
    public int UnreadCount { get; private set; }
    public bool IsInitialized { get; private set; }

    public event Action? OnChange;

    public void SetUnreadCount(int unreadCount)
    {
        UnreadCount = Math.Max(0, unreadCount);
        IsInitialized = true;
        OnChange?.Invoke();
    }

    public void DecrementUnread()
    {
        SetUnreadCount(Math.Max(0, UnreadCount - 1));
    }

    public void Clear()
    {
        UnreadCount = 0;
        IsInitialized = true;
        OnChange?.Invoke();
    }

    public void Reset()
    {
        UnreadCount = 0;
        IsInitialized = false;
        OnChange?.Invoke();
    }
}
