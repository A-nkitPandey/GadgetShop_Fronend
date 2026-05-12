namespace GadgetShop.Models;

public sealed class BackendPaginationResponse<T>
{
    public long TotalCount { get; set; }
    public T? Records { get; set; }
}

public sealed class BackendCustomerCatalogListItem
{
    public long Id { get; set; }
    public string ProductCode { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public long? CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public long? BrandId { get; set; }
    public string? BrandName { get; set; }
    public decimal BasePrice { get; set; }
    public decimal? Mrp { get; set; }
    public string CurrencyCode { get; set; } = "INR";
    public string? ImageUrl { get; set; }
    public bool HasVariants { get; set; }
    public bool InStock { get; set; }
}

public sealed class BackendProductDetail
{
    public long Id { get; set; }
    public long? CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public long? BrandId { get; set; }
    public string? BrandName { get; set; }
    public decimal BasePrice { get; set; }
    public decimal? Mrp { get; set; }
    public string CurrencyCode { get; set; } = "INR";
    public int StockQuantity { get; set; }
    public int AvailableQuantity { get; set; }
    public string ProductCode { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public List<BackendProductGalleryImage> GalleryImages { get; set; } = new();
    public bool HasVariants { get; set; }
    public List<BackendProductVariantSummary> Variants { get; set; } = new();
}

public sealed class BackendProductGalleryImage
{
    public long Id { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
    public bool IsPrimary { get; set; }
}

public sealed class BackendProductVariantSummary
{
    public long Id { get; set; }
    public string SkuCode { get; set; } = string.Empty;
    public string VariantName { get; set; } = string.Empty;
    public string AttributeSummary { get; set; } = string.Empty;
    public decimal BasePrice { get; set; }
    public decimal? Mrp { get; set; }
    public string CurrencyCode { get; set; } = "INR";
    public int StockQuantity { get; set; }
    public int AvailableQuantity { get; set; }
}

public sealed class BackendRecommendationResponse
{
    public List<BackendRecommendationSection> Sections { get; set; } = new();
}

public sealed class BackendRecommendationSection
{
    public string Title { get; set; } = string.Empty;
    public List<BackendCustomerCatalogListItem> Products { get; set; } = new();
}

public sealed class BackendCart
{
    public long CartId { get; set; }
    public string CartCode { get; set; } = string.Empty;
    public string CartStatus { get; set; } = string.Empty;
    public string CurrencyCode { get; set; } = "INR";
    public int TotalQuantity { get; set; }
    public decimal SubTotal { get; set; }
    public string? CouponCode { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public bool HasExpiredReservations { get; set; }
    public List<BackendCartItem> Items { get; set; } = new();
}

public sealed class BackendCartItem
{
    public long Id { get; set; }
    public long ProductId { get; set; }
    public long? VariantId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string? VariantName { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal? Mrp { get; set; }
    public decimal LineTotal { get; set; }
    public string CurrencyCode { get; set; } = "INR";
    public bool IsReservationActive { get; set; }
    public DateTime? ReservationExpiresAt { get; set; }
    public int AvailableQuantity { get; set; }
}

public sealed class BackendCartMutation
{
    public long CartId { get; set; }
    public long CartItemId { get; set; }
    public int Quantity { get; set; }
    public bool IsReservationActive { get; set; }
    public DateTime? ReservationExpiresAt { get; set; }
    public string Message { get; set; } = string.Empty;
}

public sealed class BackendWishlistItem
{
    public long Id { get; set; }
    public long ProductId { get; set; }
    public long? VariantId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string? VariantName { get; set; }
    public decimal BasePrice { get; set; }
    public decimal? Mrp { get; set; }
    public string CurrencyCode { get; set; } = "INR";
    public string? ImageUrl { get; set; }
    public bool InStock { get; set; }
}

public sealed class BackendAddressRequest
{
    public long Id { get; set; }
    public string AddressLabel { get; set; } = string.Empty;
    public string RecipientName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string AddressLine1 { get; set; } = string.Empty;
    public string? AddressLine2 { get; set; }
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public string CountryCode { get; set; } = "IN";
    public bool IsDefaultShipping { get; set; }
    public bool IsDefaultBilling { get; set; }
}

public sealed class BackendAddress
{
    public long Id { get; set; }
    public string AddressLabel { get; set; } = string.Empty;
    public string RecipientName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string AddressLine1 { get; set; } = string.Empty;
    public string? AddressLine2 { get; set; }
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public string CountryCode { get; set; } = "IN";
    public bool IsDefaultShipping { get; set; }
    public bool IsDefaultBilling { get; set; }
}

public sealed class BackendOrderGrid
{
    public long Id { get; set; }
    public string OrderNo { get; set; } = string.Empty;
    public string? FullName { get; set; }
    public string OrderStatus { get; set; } = string.Empty;
    public string PaymentStatus { get; set; } = string.Empty;
    public int TotalQuantity { get; set; }
    public decimal TotalAmount { get; set; }
    public string CurrencyCode { get; set; } = "INR";
    public DateTime PlacedAt { get; set; }
}

public sealed class BackendOrderDetail
{
    public long Id { get; set; }
    public string OrderNo { get; set; } = string.Empty;
    public string? FullName { get; set; }
    public string OrderStatus { get; set; } = string.Empty;
    public string PaymentStatus { get; set; } = string.Empty;
    public string CurrencyCode { get; set; } = "INR";
    public decimal SubTotal { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public string? CouponCode { get; set; }
    public string? CustomerNote { get; set; }
    public DateTime PlacedAt { get; set; }
    public List<BackendOrderItem> Items { get; set; } = new();
}

public sealed class BackendOrderItem
{
    public long Id { get; set; }
    public long ProductId { get; set; }
    public long? VariantId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string? VariantName { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal? Mrp { get; set; }
    public decimal LineTotal { get; set; }
    public string CurrencyCode { get; set; } = "INR";
}

public sealed class BackendPlaceOrderResponse
{
    public long OrderId { get; set; }
    public string OrderNo { get; set; } = string.Empty;
    public string OrderStatus { get; set; } = string.Empty;
    public string PaymentStatus { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public string CurrencyCode { get; set; } = "INR";
    public string Message { get; set; } = string.Empty;
}

public sealed class BackendProductReview
{
    public long Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public int Rating { get; set; }
    public string? ReviewTitle { get; set; }
    public string? ReviewText { get; set; }
    public DateTime CreatedAt { get; set; }
}

public sealed class BackendPaymentOrder
{
    public long PaymentId { get; set; }
    public long OrderId { get; set; }
    public string OrderNo { get; set; } = string.Empty;
    public string Provider { get; set; } = string.Empty;
    public string KeyId { get; set; } = string.Empty;
    public string ProviderOrderId { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public long AmountInSubunits { get; set; }
    public string Currency { get; set; } = "INR";
    public string AppName { get; set; } = string.Empty;
    public string PaymentStatus { get; set; } = string.Empty;
}

public sealed class BackendVerifyPaymentResponse
{
    public long PaymentId { get; set; }
    public long OrderId { get; set; }
    public string PaymentStatus { get; set; } = string.Empty;
    public string OrderStatus { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}

public sealed class BackendAdminDashboardSummary
{
    public int TotalProducts { get; set; }
    public int ActiveProducts { get; set; }
    public int LowStockProducts { get; set; }
    public int TotalOrders { get; set; }
    public int PendingOrders { get; set; }
    public decimal TotalSales { get; set; }
}

public sealed class BackendAdminDashboardOverview
{
    public BackendAdminDashboardSummary Summary { get; set; } = new();
    public List<BackendAdminDashboardRecentOrder> RecentOrders { get; set; } = new();
    public List<BackendAdminDashboardLowStockProduct> LowStockProducts { get; set; } = new();
    public List<BackendAdminDashboardTopSellingProduct> TopSellingProducts { get; set; } = new();
}

public sealed class BackendAdminDashboardTopSellingProduct
{
    public string ProductName { get; set; } = string.Empty;
    public int TotalQuantitySold { get; set; }
    public decimal TotalSalesAmount { get; set; }
}

public sealed class BackendAdminDashboardRecentOrder
{
    public long OrderId { get; set; }
    public string OrderNo { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string OrderStatus { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public DateTime PlacedAt { get; set; }
}

public sealed class BackendAdminDashboardLowStockProduct
{
    public string ProductName { get; set; } = string.Empty;
    public int AvailableQuantity { get; set; }
}

public sealed class BackendProductGrid
{
    public long Id { get; set; }
    public string ProductCode { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public string? CategoryName { get; set; }
    public string? BrandName { get; set; }
    public decimal BasePrice { get; set; }
    public int StockQuantity { get; set; }
    public bool IsActive { get; set; }
    public string? ImageUrl { get; set; }
}

public sealed class BackendCategoryGrid
{
    public long Id { get; set; }
    public string CategoryCode { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
    public string? ParentCategoryName { get; set; }
    public bool IsActive { get; set; }
}

public sealed class BackendBrandGrid
{
    public long Id { get; set; }
    public string BrandCode { get; set; } = string.Empty;
    public string BrandName { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}

public sealed class BackendCouponGrid
{
    public long Id { get; set; }
    public string CouponCode { get; set; } = string.Empty;
    public string CouponName { get; set; } = string.Empty;
    public string DiscountType { get; set; } = string.Empty;
    public decimal DiscountValue { get; set; }
    public decimal? MinimumOrderAmount { get; set; }
    public decimal? MaximumDiscountAmount { get; set; }
    public DateTime ValidFrom { get; set; }
    public DateTime ValidTo { get; set; }
    public bool IsActive { get; set; }
}

public sealed class BackendAdminUserGrid
{
    public long Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string RoleCode { get; set; } = string.Empty;
    public string RoleName { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}

public sealed class BackendInventoryTransaction
{
    public long Id { get; set; }
    public string ProductCode { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public string? VariantName { get; set; }
    public string TransactionType { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public string ReferenceNo { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
}

public sealed class BackendInventoryReport
{
    public BackendInventoryOverview Overview { get; set; } = new();
    public List<BackendInventoryLowStockItem> LowStockItems { get; set; } = new();
}

public sealed class BackendInventoryOverview
{
    public long LowStockProducts { get; set; }
    public int TotalAvailableUnits { get; set; }
}

public sealed class BackendInventoryLowStockItem
{
    public long ProductId { get; set; }
    public string ProductCode { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public string? VariantName { get; set; }
    public int AvailableQuantity { get; set; }
    public int ReorderLevel { get; set; }
}

public sealed class BackendAuditLogGrid
{
    public long Id { get; set; }
    public string HttpMethod { get; set; } = string.Empty;
    public string ControllerName { get; set; } = string.Empty;
    public string ActionName { get; set; } = string.Empty;
    public string? UserCode { get; set; }
    public int? ResponseCode { get; set; }
    public bool IsSuccess { get; set; }
    public DateTime CreatedAt { get; set; }
}

public sealed class BackendInvoiceAdminGrid
{
    public long Id { get; set; }
    public long OrderId { get; set; }
    public string OrderNo { get; set; } = string.Empty;
    public string InvoiceNo { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public decimal GrandTotal { get; set; }
    public DateTime GeneratedAt { get; set; }
}

public sealed class BackendPaymentAdminGrid
{
    public long Id { get; set; }
    public string PaymentNo { get; set; } = string.Empty;
    public string OrderNo { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public decimal RefundedAmount { get; set; }
    public string CurrencyCode { get; set; } = string.Empty;
    public string GatewayName { get; set; } = string.Empty;
    public string? TransactionId { get; set; }
    public string PaymentStatus { get; set; } = string.Empty;
    public DateTime? PaymentDate { get; set; }
    public DateTime? RefundedAt { get; set; }
}
