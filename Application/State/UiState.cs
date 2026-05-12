namespace GadgetShop.State;

public sealed class UiState
{
    public bool IsDarkMode { get; private set; }
    public event Action? OnChange;

    public void ToggleTheme()
    {
        IsDarkMode = !IsDarkMode;
        OnChange?.Invoke();
    }
}
