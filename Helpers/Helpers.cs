namespace GadgetShop.Helpers;

// ─── App Constants ────────────────────────────────────────────
public static class AppConstants
{
    public const string AppName = "GadgetShop";
    public const string AdminRole = "Admin";
    public const string SuperAdminRole = "SuperAdmin";
    public const string CustomerRole = "Customer";

    public static readonly string[] AdminRoles = { AdminRole, SuperAdminRole };

    public static class OrderStatus
    {
        public const string Pending = "Pending";
        public const string Confirmed = "Confirmed";
        public const string Processing = "Processing";
        public const string Shipped = "Shipped";
        public const string Delivered = "Delivered";
        public const string Cancelled = "Cancelled";

        public static readonly string[] All = { Pending, Confirmed, Processing, Shipped, Delivered, Cancelled };
    }
}
