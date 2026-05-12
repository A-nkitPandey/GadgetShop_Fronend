namespace GadgetShop.Constants;

public static class AppRoles
{
    public const string Admin = "Admin";
    public const string SuperAdmin = "SuperAdmin";
    public const string NormalUser = "NormalUser";

    public static bool IsAdminRole(string? role) =>
        role is Admin or SuperAdmin;

    public static bool IsAdmin(IEnumerable<string>? roles) =>
        roles?.Any(IsAdminRole) == true;
}
