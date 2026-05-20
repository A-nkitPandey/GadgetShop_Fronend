namespace GadgetShop.Helpers;

public readonly record struct StatusOption(string Value, string Label);

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
        public const string Placed = "PLACED";
        public const string Processing = "PROCESSING";
        public const string Shipped = "SHIPPED";
        public const string Delivered = "DELIVERED";
        public const string Cancelled = "CANCELLED";

        public static readonly StatusOption[] Options =
        {
            new(Placed, "Placed"),
            new(Processing, "Processing"),
            new(Shipped, "Shipped"),
            new(Delivered, "Delivered"),
            new(Cancelled, "Cancelled")
        };

        public static readonly string[] All = { Placed, Processing, Shipped, Delivered, Cancelled };

        public static string GetLabel(string? value) => value?.ToUpperInvariant() switch
        {
            Placed => "Placed",
            Processing => "Processing",
            Shipped => "Shipped",
            Delivered => "Delivered",
            Cancelled => "Cancelled",
            _ => value ?? "Unknown"
        };
    }

    public static class ShipmentStatus
    {
        public const string Pending = "PENDING";
        public const string Dispatched = "DISPATCHED";
        public const string Delivered = "DELIVERED";
        public static readonly StatusOption[] Options =
        {
            new(Pending, "Pending"),
            new(Dispatched, "Dispatched"),
            new(Delivered, "Delivered")
        };

        public static readonly string[] All = { Pending, Dispatched, Delivered };

        public static string GetLabel(string? value) => value?.ToUpperInvariant() switch
        {
            Pending => "Pending",
            Dispatched => "Dispatched",
            Delivered => "Delivered",
            _ => value ?? "Unknown"
        };
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
