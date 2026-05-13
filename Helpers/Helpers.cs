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

    public static class ShipmentStatus
    {
        public const string Pending = "PENDING";
        public const string Dispatched = "DISPATCHED";
        public const string Delivered = "DELIVERED";
        public static readonly string[] All = { Pending, Dispatched, Delivered };
    }

    public static class ReturnStatus
    {
        public const string Requested = "REQUESTED";
        public const string Approved = "APPROVED";
        public const string Rejected = "REJECTED";
        public const string Completed = "COMPLETED";
        public static readonly string[] All = { Requested, Approved, Rejected, Completed };
    }
}
