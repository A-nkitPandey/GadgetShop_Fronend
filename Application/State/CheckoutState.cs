namespace GadgetShop.State;

public sealed class CheckoutState
{
    public long? SelectedAddressId { get; private set; }
    public string? Notes { get; private set; }
    public event Action? OnChange;

    public void SetSelection(long? addressId, string? notes)
    {
        SelectedAddressId = addressId;
        Notes = notes;
        OnChange?.Invoke();
    }

    public void Clear()
    {
        SelectedAddressId = null;
        Notes = null;
        OnChange?.Invoke();
    }
}
