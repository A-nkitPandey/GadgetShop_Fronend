// ============================================================
// GadgetShop.Models  –  Full backend contract mirror
// ============================================================
using System.ComponentModel.DataAnnotations;

namespace GadgetShop.Models;

// ─── Generic wrappers ───────────────────────────────────────
public class ApiResponse<T>
{
    public int Code { get; set; }
    public string? Message { get; set; }
    public List<string> Errors { get; set; } = new();
    public T? Data { get; set; }
    public bool IsSuccess => Code is >= 200 and < 300 && Errors.Count == 0;
}

public sealed class PagedResult<T>
{
    public List<T> Items { get; set; } = new();
    public int PageNo { get; set; }
    public int PageSize { get; set; }
    public int TotalRecords { get; set; }
    public int TotalPages { get; set; }
}

public class PaginationRequest
{
    [Range(1, int.MaxValue)] public int PageNo { get; set; } = 1;
    [Range(10, int.MaxValue)] public int PageSize { get; set; } = 20;
    public string? SortByColumn { get; set; }
    public bool SortDesOrder { get; set; }
    public string? SearchText { get; set; }
    public string? SearchByColumn { get; set; }
    public List<SearchDomain> Searches { get; set; } = new();
}

public sealed class SearchDomain
{
    public string? ColumnName { get; set; }
    public string? SearchText { get; set; }
    public string? SearchOperator { get; set; }
}

public sealed class LookupItem
{
    public long Id { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
}

// ─── Auth ───────────────────────────────────────────────────
public sealed class LoginRequest
{
    [Required] public string UserName { get; set; } = string.Empty;
    [Required] public string Password { get; set; } = string.Empty;
}

public sealed class LoginResponse
{
    public string? Token { get; set; }
    public string? RefreshToken { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public List<string> Roles { get; set; } = new();
}

public sealed class CustomerRegisterRequest
{
    [Required, StringLength(100)] public string FullName { get; set; } = string.Empty;
    [Required, EmailAddress] public string Email { get; set; } = string.Empty;
    [Required, Phone] public string PhoneNumber { get; set; } = string.Empty;
    [Required, MinLength(6)] public string Password { get; set; } = string.Empty;
    [Required, Compare(nameof(Password))] public string ConfirmPassword { get; set; } = string.Empty;
}

public sealed class CustomerProfileUpdateRequest
{
    [Required, StringLength(100)] public string FullName { get; set; } = string.Empty;
    [Phone] public string? PhoneNumber { get; set; }
}

public sealed class ChangePasswordRequest
{
    [Required] public string CurrentPassword { get; set; } = string.Empty;
    [Required, MinLength(6)] public string NewPassword { get; set; } = string.Empty;
    [Required, Compare(nameof(NewPassword))] public string ConfirmNewPassword { get; set; } = string.Empty;
}

// ─── Customer Catalog ───────────────────────────────────────
public sealed class CustomerCatalogListRequest
{
    public int PageNo { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public string? SearchText { get; set; }
    public long? CategoryId { get; set; }
    public long? BrandId { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public string? SortBy { get; set; }
    public bool SortDesc { get; set; }
}

public sealed class CustomerCatalogGetByIdRequest
{
    [Range(1, long.MaxValue)] public long Id { get; set; }
}

public sealed class CustomerCatalogProductDto
{
    public long Id { get; set; }
    public string? ProductCode { get; set; }
    public string? ProductName { get; set; }
    public string? Description { get; set; }
    public decimal BasePrice { get; set; }
    public decimal? Mrp { get; set; }
    public string? CurrencyCode { get; set; }
    public int StockQuantity { get; set; }
    public bool IsInStock { get; set; }
    public string? PrimaryImageUrl { get; set; }
    public string? CategoryName { get; set; }
    public string? BrandName { get; set; }
    public double? AverageRating { get; set; }
    public int ReviewCount { get; set; }
    public List<ProductVariantDto> Variants { get; set; } = new();
    public List<string> GalleryImages { get; set; } = new();
}

public sealed class ProductVariantDto
{
    public long Id { get; set; }
    public string? SkuCode { get; set; }
    public string? VariantName { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public string? ImageUrl { get; set; }
    public List<VariantAttributeDto> Attributes { get; set; } = new();
}

public sealed class VariantAttributeDto
{
    public string? AttributeName { get; set; }
    public string? ValueText { get; set; }
}

// ─── Cart ───────────────────────────────────────────────────
public sealed class AddCartItemRequest
{
    [Range(1, long.MaxValue)] public long ProductId { get; set; }
    public long? VariantId { get; set; }
    [Range(1, int.MaxValue)] public int Quantity { get; set; } = 1;
}

public sealed class UpdateCartItemQuantityRequest
{
    [Range(1, long.MaxValue)] public long CartItemId { get; set; }
    [Range(1, int.MaxValue)] public int Quantity { get; set; }
}

public sealed class RemoveCartItemRequest
{
    [Range(1, long.MaxValue)] public long CartItemId { get; set; }
}

public sealed class ApplyCartCouponRequest
{
    [Required] public string CouponCode { get; set; } = string.Empty;
}

public sealed class CartDto
{
    public long Id { get; set; }
    public List<CartItemDto> Items { get; set; } = new();
    public decimal SubTotal { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public string? AppliedCouponCode { get; set; }
    public string? CurrencyCode { get; set; }
}

public sealed class CartItemDto
{
    public long Id { get; set; }
    public long ProductId { get; set; }
    public long? VariantId { get; set; }
    public string? ProductName { get; set; }
    public string? VariantName { get; set; }
    public string? ImageUrl { get; set; }
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }
    public int AvailableStock { get; set; }
}

// ─── Wishlist ───────────────────────────────────────────────
public sealed class WishlistMutationRequest
{
    [Range(1, long.MaxValue)] public long ProductId { get; set; }
}

public sealed class WishlistRemoveRequest
{
    [Range(1, long.MaxValue)] public long ProductId { get; set; }
}

public sealed class WishlistItemDto
{
    public long ProductId { get; set; }
    public string? ProductName { get; set; }
    public string? ImageUrl { get; set; }
    public decimal Price { get; set; }
    public bool IsInStock { get; set; }
}

// ─── Order ──────────────────────────────────────────────────
public sealed class PlaceOrderRequest
{
    [Range(1, long.MaxValue)] public long AddressId { get; set; }
    public string? Notes { get; set; }
}

public sealed class OrderGetByIdRequest
{
    [Range(1, long.MaxValue)] public long OrderId { get; set; }
}

public sealed class OrderListRequest
{
    public int PageNo { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public string? Status { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}

public sealed class CancelMyOrderRequest
{
    [Range(1, long.MaxValue)] public long OrderId { get; set; }
    public string? Remarks { get; set; }
}

public sealed class ReorderRequest
{
    [Range(1, long.MaxValue)] public long OrderId { get; set; }
}

public sealed class OrderDto
{
    public long Id { get; set; }
    public string? OrderNo { get; set; }
    public string? Status { get; set; }
    public decimal TotalAmount { get; set; }
    public string? CurrencyCode { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<OrderItemDto> Items { get; set; } = new();
    public OrderAddressDto? DeliveryAddress { get; set; }
    public string? PaymentStatus { get; set; }
    public string? TrackingNo { get; set; }
}

public sealed class OrderItemDto
{
    public long Id { get; set; }
    public long ProductId { get; set; }
    public string? ProductName { get; set; }
    public string? ImageUrl { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
}

public sealed class OrderAddressDto
{
    public string? FullName { get; set; }
    public string? AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? PinCode { get; set; }
    public string? PhoneNumber { get; set; }
}

// ─── Address ─────────────────────────────────────────────────
public sealed class CustomerAddressMutationRequest
{
    public long? Id { get; set; }
    [Required, StringLength(100)] public string FullName { get; set; } = string.Empty;
    [Required] public string AddressLine1 { get; set; } = string.Empty;
    public string? AddressLine2 { get; set; }
    [Required] public string City { get; set; } = string.Empty;
    [Required] public string State { get; set; } = string.Empty;
    [Required] public string PinCode { get; set; } = string.Empty;
    [Required, Phone] public string PhoneNumber { get; set; } = string.Empty;
    public bool IsDefault { get; set; }
}

public sealed class CustomerAddressDeleteRequest
{
    [Range(1, long.MaxValue)] public long Id { get; set; }
}

public sealed class CustomerAddressDto
{
    public long Id { get; set; }
    public string? FullName { get; set; }
    public string? AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? PinCode { get; set; }
    public string? PhoneNumber { get; set; }
    public bool IsDefault { get; set; }
}

// ─── Payment ─────────────────────────────────────────────────
public sealed class CreatePaymentOrderRequest
{
    [Range(1, long.MaxValue)] public long OrderId { get; set; }
}

public sealed class VerifyPaymentRequest
{
    public string? RazorpayOrderId { get; set; }
    public string? RazorpayPaymentId { get; set; }
    public string? RazorpaySignature { get; set; }
    public long OrderId { get; set; }
}

public sealed class RetryPaymentRequest
{
    [Range(1, long.MaxValue)] public long OrderId { get; set; }
}

public sealed class PaymentOrderDto
{
    public string? GatewayOrderId { get; set; }
    public decimal Amount { get; set; }
    public string? Currency { get; set; }
    public string? GatewayKey { get; set; }
    public long OrderId { get; set; }
    public string? UserEmail { get; set; }
    public string? UserName { get; set; }
    public string? UserPhone { get; set; }
}

// ─── Reviews ─────────────────────────────────────────────────
public sealed class ProductReviewMutationRequest
{
    [Range(1, long.MaxValue)] public long ProductId { get; set; }
    [Range(1, 5)] public int Rating { get; set; }
    [Required, StringLength(1000)] public string ReviewText { get; set; } = string.Empty;
}

public sealed class ProductReviewDeleteRequest
{
    [Range(1, long.MaxValue)] public long ReviewId { get; set; }
}

public sealed class ProductReviewListRequest
{
    [Range(1, long.MaxValue)] public long ProductId { get; set; }
    public int PageNo { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public sealed class ProductReviewDto
{
    public long Id { get; set; }
    public string? ReviewerName { get; set; }
    public int Rating { get; set; }
    public string? ReviewText { get; set; }
    public DateTime CreatedAt { get; set; }
}

// ─── Admin – Dashboard ───────────────────────────────────────
public sealed class AdminDashboardReportRequest
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int TopCount { get; set; } = 5;
}

public sealed class AdminDashboardSummaryDto
{
    public int TotalOrders { get; set; }
    public decimal TotalRevenue { get; set; }
    public int TotalProducts { get; set; }
    public int TotalCustomers { get; set; }
    public int PendingOrders { get; set; }
    public int LowStockProducts { get; set; }
}

public sealed class AdminDashboardOverviewDto
{
    public List<SalesChartPoint> MonthlySales { get; set; } = new();
    public List<TopProductDto> TopProducts { get; set; } = new();
    public List<RecentOrderDto> RecentOrders { get; set; } = new();
}

public sealed class SalesChartPoint
{
    public string? Label { get; set; }
    public decimal Amount { get; set; }
    public int OrderCount { get; set; }
}

public sealed class TopProductDto
{
    public string? ProductName { get; set; }
    public int SoldQty { get; set; }
    public decimal Revenue { get; set; }
}

public sealed class RecentOrderDto
{
    public long OrderId { get; set; }
    public string? OrderNo { get; set; }
    public string? CustomerName { get; set; }
    public decimal Amount { get; set; }
    public string? Status { get; set; }
    public DateTime CreatedAt { get; set; }
}

// ─── Admin – Products ────────────────────────────────────────
public class CreateProductRequest
{
    public long? CategoryId { get; set; }
    public long? BrandId { get; set; }
    [Range(typeof(decimal), "0.01", "999999999")] public decimal BasePrice { get; set; }
    public decimal? Mrp { get; set; }
    public decimal? CostPrice { get; set; }
    [Required, StringLength(10)] public string CurrencyCode { get; set; } = "INR";
    public bool TrackInventory { get; set; }
    public int StockQuantity { get; set; }
    public int ReservedQuantity { get; set; }
    public int ReorderLevel { get; set; }
    [Required, StringLength(50)] public string ProductCode { get; set; } = string.Empty;
    [Required, StringLength(150)] public string ProductName { get; set; } = string.Empty;
    [StringLength(2000)] public string? Description { get; set; }
}

public sealed class UpdateProductRequest : CreateProductRequest
{
    [Range(1, long.MaxValue)] public long Id { get; set; }
}

public sealed class ProductGetByIdRequest
{
    [Range(1, long.MaxValue)] public long Id { get; set; }
}

public sealed class ProductStatusUpdateRequest
{
    [Range(1, long.MaxValue)] public long Id { get; set; }
    public bool IsActive { get; set; }
}

public sealed class AdminProductDto
{
    public long Id { get; set; }
    public string? ProductCode { get; set; }
    public string? ProductName { get; set; }
    public string? CategoryName { get; set; }
    public string? BrandName { get; set; }
    public decimal BasePrice { get; set; }
    public int StockQuantity { get; set; }
    public bool IsActive { get; set; }
    public string? PrimaryImageUrl { get; set; }
    public DateTime CreatedAt { get; set; }
}

// ─── Admin – Categories ──────────────────────────────────────
public class CreateCategoryRequest
{
    [Required, StringLength(50)] public string CategoryCode { get; set; } = string.Empty;
    [Required, StringLength(150)] public string CategoryName { get; set; } = string.Empty;
    [StringLength(150)] public string? Slug { get; set; }
    [StringLength(500)] public string? Description { get; set; }
    [Range(0, int.MaxValue)] public int DisplayOrder { get; set; }
    public long? ParentCategoryId { get; set; }
}

public sealed class UpdateCategoryRequest : CreateCategoryRequest
{
    [Range(1, long.MaxValue)] public long Id { get; set; }
}

public sealed class CategoryGetByIdRequest { [Range(1, long.MaxValue)] public long Id { get; set; } }
public sealed class CategoryStatusUpdateRequest { [Range(1, long.MaxValue)] public long Id { get; set; } public bool IsActive { get; set; } }

public sealed class CategoryDto
{
    public long Id { get; set; }
    public string? CategoryCode { get; set; }
    public string? CategoryName { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public int DisplayOrder { get; set; }
    public string? ParentCategoryName { get; set; }
}

// ─── Admin – Brands ──────────────────────────────────────────
public class CreateBrandRequest
{
    [Required, StringLength(50)] public string BrandCode { get; set; } = string.Empty;
    [Required, StringLength(150)] public string BrandName { get; set; } = string.Empty;
    [StringLength(150)] public string? Slug { get; set; }
    [StringLength(500)] public string? Description { get; set; }
}

public sealed class UpdateBrandRequest : CreateBrandRequest
{
    [Range(1, long.MaxValue)] public long Id { get; set; }
}

public sealed class BrandGetByIdRequest { [Range(1, long.MaxValue)] public long Id { get; set; } }
public sealed class BrandStatusUpdateRequest { [Range(1, long.MaxValue)] public long Id { get; set; } public bool IsActive { get; set; } }

public sealed class BrandDto
{
    public long Id { get; set; }
    public string? BrandCode { get; set; }
    public string? BrandName { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
}

// ─── Admin – Coupons ─────────────────────────────────────────
public class CreateCouponRequest
{
    [Required] public string CouponCode { get; set; } = string.Empty;
    [Required] public string CouponName { get; set; } = string.Empty;
    [Required] public string DiscountType { get; set; } = "Percentage";
    public decimal DiscountValue { get; set; }
    public decimal? MinimumOrderAmount { get; set; }
    public decimal? MaximumDiscountAmount { get; set; }
    public DateTime ValidFrom { get; set; } = DateTime.Today;
    public DateTime ValidTo { get; set; } = DateTime.Today.AddMonths(1);
    public string? Description { get; set; }
}

public sealed class UpdateCouponRequest : CreateCouponRequest
{
    [Range(1, long.MaxValue)] public long Id { get; set; }
}

public sealed class CouponGetByIdRequest { [Range(1, long.MaxValue)] public long Id { get; set; } }
public sealed class CouponStatusUpdateRequest { [Range(1, long.MaxValue)] public long Id { get; set; } public bool IsActive { get; set; } }

public sealed class CouponDto
{
    public long Id { get; set; }
    public string? CouponCode { get; set; }
    public string? CouponName { get; set; }
    public string? DiscountType { get; set; }
    public decimal DiscountValue { get; set; }
    public DateTime ValidFrom { get; set; }
    public DateTime ValidTo { get; set; }
    public bool IsActive { get; set; }
}

// ─── Admin – Users ───────────────────────────────────────────
public sealed class CreateAdminUserRequest
{
    [Required, StringLength(100)] public string FullName { get; set; } = string.Empty;
    [Required, EmailAddress] public string Email { get; set; } = string.Empty;
    [Required] public string Password { get; set; } = string.Empty;
    public List<string> Roles { get; set; } = new();
}

public sealed class UpdateAdminUserRequest
{
    [Range(1, long.MaxValue)] public long Id { get; set; }
    [Required, StringLength(100)] public string FullName { get; set; } = string.Empty;
    [Required, EmailAddress] public string Email { get; set; } = string.Empty;
    public List<string> Roles { get; set; } = new();
}

public sealed class AdminUserGetByIdRequest { [Range(1, long.MaxValue)] public long Id { get; set; } }
public sealed class AdminUserListRequest : PaginationRequest { }
public sealed class AdminUserStatusUpdateRequest { [Range(1, long.MaxValue)] public long Id { get; set; } public bool IsActive { get; set; } }
public sealed class AdminUserArchiveRequest { [Range(1, long.MaxValue)] public long Id { get; set; } }
public sealed class AdminUserPasswordResetRequest { [Range(1, long.MaxValue)] public long Id { get; set; } [Required] public string NewPassword { get; set; } = string.Empty; }

public sealed class AdminUserDto
{
    public long Id { get; set; }
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public bool IsActive { get; set; }
    public List<string> Roles { get; set; } = new();
    public DateTime CreatedAt { get; set; }
}

// ─── Admin – Orders ──────────────────────────────────────────
public sealed class UpdateOrderStatusRequest
{
    [Range(1, long.MaxValue)] public long OrderId { get; set; }
    [Required] public string Status { get; set; } = string.Empty;
    public string? Remarks { get; set; }
}

public sealed class CreateOrderShipmentRequest
{
    [Range(1, long.MaxValue)] public long OrderId { get; set; }
    [Required, StringLength(100)] public string CourierPartner { get; set; } = string.Empty;
    [StringLength(100)] public string? TrackingNo { get; set; }
    [StringLength(500)] public string? Remarks { get; set; }
}

public sealed class UpdateOrderShipmentStatusRequest
{
    [Range(1, long.MaxValue)] public long ShipmentId { get; set; }
    [Required] public string Status { get; set; } = string.Empty;
    public string? Remarks { get; set; }
}

public sealed class OrderShipmentGetByOrderIdRequest { [Range(1, long.MaxValue)] public long OrderId { get; set; } }
public sealed class OrderShipmentListRequest : PaginationRequest { }
public sealed class OrderReturnGetByIdRequest { [Range(1, long.MaxValue)] public long ReturnId { get; set; } }
public sealed class OrderReturnListRequest : PaginationRequest { }
public sealed class UpdateOrderReturnStatusRequest { [Range(1, long.MaxValue)] public long ReturnId { get; set; } [Required] public string Status { get; set; } = string.Empty; }
public sealed class MyReturnListRequest : PaginationRequest { }
public sealed class CreateOrderReturnRequest
{
    [Range(1, long.MaxValue)] public long OrderId { get; set; }
    [Range(1, long.MaxValue)] public long OrderItemId { get; set; }
    [Range(1, int.MaxValue)] public int Quantity { get; set; }
    [Required, StringLength(500)] public string Reason { get; set; } = string.Empty;
}

// ─── Admin – Inventory ───────────────────────────────────────
public sealed class InventoryAdjustmentRequest
{
    [Range(1, long.MaxValue)] public long VariantId { get; set; }
    [Range(1, int.MaxValue)] public int Quantity { get; set; }
    public string? Remarks { get; set; }
}

public sealed class ProductInventoryTransactionListRequest : PaginationRequest
{
    public long? ProductId { get; set; }
    public long? VariantId { get; set; }
}

public sealed class ProductInventoryTransactionGetByIdRequest { [Range(1, long.MaxValue)] public long Id { get; set; } }

public sealed class InventoryAdminReportRequest
{
    public int PageNo { get; set; } = 1;
    public int PageSize { get; set; } = 50;
    public bool LowStockOnly { get; set; }
}

// ─── Admin – Payment Admin ───────────────────────────────────
public sealed class PaymentGetByIdRequest { [Range(1, long.MaxValue)] public long Id { get; set; } }
public sealed class PaymentStatusUpdateRequest { [Range(1, long.MaxValue)] public long Id { get; set; } [Required] public string Status { get; set; } = string.Empty; }
public sealed class ProcessPaymentRefundRequest
{
    [Range(1, long.MaxValue)] public long PaymentId { get; set; }
    [Range(typeof(decimal), "0.01", "999999999")] public decimal RefundAmount { get; set; }
    public string? Reason { get; set; }
}

// ─── Admin – Audit Log ───────────────────────────────────────
public sealed class AuditLogListRequest : PaginationRequest { }
public sealed class AuditLogGetByIdRequest { [Range(1, long.MaxValue)] public long Id { get; set; } }

// ─── Product Variants ────────────────────────────────────────
public class CreateProductVariantRequest
{
    [Range(1, long.MaxValue)] public long ProductId { get; set; }
    [Required, StringLength(50)] public string SkuCode { get; set; } = string.Empty;
    [Required, StringLength(150)] public string VariantName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
}

public sealed class UpdateProductVariantRequest : CreateProductVariantRequest
{
    [Range(1, long.MaxValue)] public long Id { get; set; }
}

public sealed class ProductVariantGetByIdRequest { [Range(1, long.MaxValue)] public long Id { get; set; } }
public sealed class ProductVariantListRequest : PaginationRequest { public long? ProductId { get; set; } }
public sealed class ProductVariantStatusUpdateRequest { [Range(1, long.MaxValue)] public long Id { get; set; } public bool IsActive { get; set; } }

// ─── Support Tickets ─────────────────────────────────────────
public sealed class SupportTicketMutationRequest
{
    [Required] public string Subject { get; set; } = string.Empty;
    [Required] public string Message { get; set; } = string.Empty;
    public long? OrderId { get; set; }
}

public sealed class SupportTicketReplyRequest
{
    [Range(1, long.MaxValue)] public long TicketId { get; set; }
    [Required] public string Message { get; set; } = string.Empty;
}

// ─── Notifications ───────────────────────────────────────────
public sealed class NotificationDto
{
    public long Id { get; set; }
    public string? Title { get; set; }
    public string? Body { get; set; }
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }
}

// ─── Invoice ─────────────────────────────────────────────────
public sealed class GenerateInvoiceRequest { [Range(1, long.MaxValue)] public long OrderId { get; set; } }
public sealed class InvoiceGetByIdRequest { [Range(1, long.MaxValue)] public long Id { get; set; } }
public sealed class InvoicePrintDocumentRequest { [Range(1, long.MaxValue)] public long InvoiceId { get; set; } }

// ─── Customer Profile ────────────────────────────────────────
public sealed class CustomerProfileDto
{
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public DateTime? CreatedAt { get; set; }
}
