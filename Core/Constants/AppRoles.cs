using System.Security.Claims;

namespace GadgetShop.Constants;

public static class AppRoles
{
    public const string Admin = "Admin";
    public const string SuperAdmin = "SuperAdmin";
    public const string NormalUser = "NormalUser";

    public static bool IsAdminRole(string? role) =>
        string.Equals(role, Admin, StringComparison.OrdinalIgnoreCase) ||
        string.Equals(role, SuperAdmin, StringComparison.OrdinalIgnoreCase);

    public static bool IsSuperAdminRole(string? role) =>
        string.Equals(role, SuperAdmin, StringComparison.OrdinalIgnoreCase);

    public static bool IsCustomerRole(string? role) =>
        string.Equals(role, NormalUser, StringComparison.OrdinalIgnoreCase);

    public static bool IsAdmin(IEnumerable<string>? roles) =>
        roles?.Any(IsAdminRole) == true;

    public static bool IsSuperAdmin(IEnumerable<string>? roles) =>
        roles?.Any(IsSuperAdminRole) == true;

    public static bool IsCustomer(IEnumerable<string>? roles) =>
        roles?.Any(IsCustomerRole) == true;

    public static bool CanUseCart(IEnumerable<string>? roles) =>
        IsCustomer(roles);

    public static bool CanUseWishlist(IEnumerable<string>? roles) =>
        IsCustomer(roles);

    public static List<string> GetRoles(ClaimsPrincipal? user) =>
        user?.Claims
            .Where(c => c.Type == ClaimTypes.Role || c.Type == "role")
            .Select(c => c.Value)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList()
        ?? new();

    public static bool CanUseCart(ClaimsPrincipal? user) =>
        user?.Identity?.IsAuthenticated == true && CanUseCart(GetRoles(user));

    public static bool CanUseWishlist(ClaimsPrincipal? user) =>
        user?.Identity?.IsAuthenticated == true && CanUseWishlist(GetRoles(user));
}
