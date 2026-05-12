using GadgetShop.Shared.Dialogs;
using MudBlazor;

namespace GadgetShop.Helpers;

public static class DialogServiceExtensions
{
    public static async Task<bool> ShowConfirmAsync(
        this IDialogService dialogService,
        string title,
        string message,
        string confirmText = "OK",
        string cancelText = "Cancel")
    {
        var parameters = new DialogParameters
        {
            [nameof(ConfirmDialog.Message)] = message,
            [nameof(ConfirmDialog.ConfirmLabel)] = confirmText,
            [nameof(ConfirmDialog.CancelLabel)] = cancelText,
            [nameof(ConfirmDialog.ShowCancel)] = true
        };

        var options = new DialogOptions
        {
            CloseOnEscapeKey = true,
            MaxWidth = MaxWidth.ExtraSmall,
            FullWidth = true
        };

        var dialog = await dialogService.ShowAsync<ConfirmDialog>(title, parameters, options);
        var result = await dialog.Result;
        return result is { Canceled: false, Data: true };
    }

    public static async Task ShowInfoAsync(
        this IDialogService dialogService,
        string title,
        string message,
        string okText = "OK")
    {
        var parameters = new DialogParameters
        {
            [nameof(ConfirmDialog.Message)] = message,
            [nameof(ConfirmDialog.ConfirmLabel)] = okText,
            [nameof(ConfirmDialog.ShowCancel)] = false
        };

        var options = new DialogOptions
        {
            CloseOnEscapeKey = true,
            MaxWidth = MaxWidth.ExtraSmall,
            FullWidth = true
        };

        var dialog = await dialogService.ShowAsync<ConfirmDialog>(title, parameters, options);
        await dialog.Result;
    }
}
