namespace GadgetShop.Services;

internal static class AssetUrlResolver
{
    public static string? Normalize(Uri? apiBaseAddress, string? url)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            return url;
        }

        if (Uri.TryCreate(url, UriKind.Absolute, out var absoluteUri))
        {
            return absoluteUri.ToString();
        }

        if (apiBaseAddress is null)
        {
            return url;
        }

        return new Uri(apiBaseAddress, url.TrimStart('/')).ToString();
    }
}
