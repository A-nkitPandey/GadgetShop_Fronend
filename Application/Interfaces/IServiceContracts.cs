using GadgetShop.Models;

namespace GadgetShop.Application.Interfaces;

public interface IAuthService
{
    Task<ApiResponse<LoginResponse>> LoginAsync(LoginRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> RegisterAsync(CustomerRegisterRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> ForgotPasswordAsync(ForgotPasswordRequest request, CancellationToken ct = default);
    Task LogoutAsync();
    Task<bool> IsAuthenticatedAsync();
    Task<bool> IsAdminAsync();
}

public interface ICatalogService
{
    Task<ApiResponse<PagedResult<CustomerCatalogProductDto>>> GetProductsAsync(CustomerCatalogListRequest request, CancellationToken ct = default);
    Task<ApiResponse<CustomerCatalogProductDto>> GetProductByIdAsync(CustomerCatalogGetByIdRequest request, CancellationToken ct = default);
    Task<ApiResponse<PagedResult<ProductReviewDto>>> GetReviewsAsync(ProductReviewListRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> SaveReviewAsync(ProductReviewMutationRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> DeleteReviewAsync(ProductReviewDeleteRequest request, CancellationToken ct = default);
}

public interface IRecommendationService
{
    Task<ApiResponse<List<CustomerCatalogProductDto>>> GetRecommendationsAsync(long? productId = null, int pageSize = 8, CancellationToken ct = default);
}

public interface ICartService
{
    Task<ApiResponse<CartDto>> GetMyCartAsync(CancellationToken ct = default);
    Task<ApiResponse<CartDto>> AddItemAsync(AddCartItemRequest request, CancellationToken ct = default);
    Task<ApiResponse<CartDto>> UpdateQuantityAsync(UpdateCartItemQuantityRequest request, CancellationToken ct = default);
    Task<ApiResponse<CartDto>> RemoveItemAsync(RemoveCartItemRequest request, CancellationToken ct = default);
    Task<ApiResponse<CartDto>> ClearCartAsync(CancellationToken ct = default);
    Task<ApiResponse<CartDto>> ApplyCouponAsync(ApplyCartCouponRequest request, CancellationToken ct = default);
    Task<ApiResponse<CartDto>> RemoveCouponAsync(CancellationToken ct = default);
}

public interface IWishlistService
{
    Task<ApiResponse<List<WishlistItemDto>>> GetMyWishlistAsync(CancellationToken ct = default);
    Task<ApiResponse<object>> AddAsync(WishlistMutationRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> RemoveAsync(WishlistRemoveRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> MoveToCartAsync(WishlistRemoveRequest request, CancellationToken ct = default);
}

public interface IOrderService
{
    Task<ApiResponse<OrderDto>> GetMyOrderByIdAsync(OrderGetByIdRequest request, CancellationToken ct = default);
    Task<ApiResponse<PagedResult<OrderDto>>> GetMyOrderListAsync(OrderListRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> CancelMyOrderAsync(CancelMyOrderRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> ReorderAsync(ReorderRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> CreateReturnAsync(CreateOrderReturnRequest request, CancellationToken ct = default);
    Task<ApiResponse<PagedResult<OrderDto>>> GetOrderListAsync(OrderListRequest request, CancellationToken ct = default);
    Task<ApiResponse<OrderDto>> GetOrderByIdAsync(OrderGetByIdRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> UpdateOrderStatusAsync(UpdateOrderStatusRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> CreateShipmentAsync(CreateOrderShipmentRequest request, CancellationToken ct = default);
}

public interface ICheckoutService
{
    Task<ApiResponse<OrderDto>> PlaceOrderAsync(PlaceOrderRequest request, CancellationToken ct = default);
}

public interface IAddressService
{
    Task<ApiResponse<List<CustomerAddressDto>>> GetMyAddressesAsync(CancellationToken ct = default);
    Task<ApiResponse<object>> SaveAddressAsync(CustomerAddressMutationRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> DeleteAddressAsync(CustomerAddressDeleteRequest request, CancellationToken ct = default);
}

public interface IUserAccountService
{
    Task<ApiResponse<CustomerProfileDto>> GetMyProfileAsync(CancellationToken ct = default);
    Task<ApiResponse<object>> UpdateMyProfileAsync(CustomerProfileUpdateRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> ChangePasswordAsync(ChangePasswordRequest request, CancellationToken ct = default);
    Task<ApiResponse<List<NotificationDto>>> GetMyNotificationsAsync(CancellationToken ct = default);
}

public interface IPaymentService
{
    Task<ApiResponse<PaymentOrderDto>> CreatePaymentOrderAsync(CreatePaymentOrderRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> VerifyPaymentAsync(VerifyPaymentRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> RetryPaymentAsync(RetryPaymentRequest request, CancellationToken ct = default);
}

public interface IAdminDashboardService
{
    Task<ApiResponse<AdminDashboardSummaryDto>> GetSummaryAsync(CancellationToken ct = default);
    Task<ApiResponse<AdminDashboardOverviewDto>> GetOverviewAsync(CancellationToken ct = default);
    Task<ApiResponse<object>> GetReportAsync(AdminDashboardReportRequest request, CancellationToken ct = default);
}

public interface IAdminProductService
{
    Task<ApiResponse<PagedResult<AdminProductDto>>> GetProductListAsync(PaginationRequest request, CancellationToken ct = default);
    Task<ApiResponse<AdminProductDto>> GetProductByIdAsync(ProductGetByIdRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> CreateProductAsync(CreateProductRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> UpdateProductAsync(UpdateProductRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> UpdateProductStatusAsync(ProductStatusUpdateRequest request, CancellationToken ct = default);
}

public interface IAdminCategoryService
{
    Task<ApiResponse<PagedResult<CategoryDto>>> GetCategoryListAsync(PaginationRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> CreateCategoryAsync(CreateCategoryRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> UpdateCategoryAsync(UpdateCategoryRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> UpdateCategoryStatusAsync(CategoryStatusUpdateRequest request, CancellationToken ct = default);
}

public interface IAdminBrandService
{
    Task<ApiResponse<PagedResult<BrandDto>>> GetBrandListAsync(PaginationRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> CreateBrandAsync(CreateBrandRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> UpdateBrandAsync(UpdateBrandRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> UpdateBrandStatusAsync(BrandStatusUpdateRequest request, CancellationToken ct = default);
}

public interface IAdminCouponService
{
    Task<ApiResponse<PagedResult<CouponDto>>> GetCouponListAsync(PaginationRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> CreateCouponAsync(CreateCouponRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> UpdateCouponAsync(UpdateCouponRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> UpdateCouponStatusAsync(CouponStatusUpdateRequest request, CancellationToken ct = default);
}

public interface IAdminOrderService
{
    Task<ApiResponse<PagedResult<OrderDto>>> GetOrderListAsync(OrderListRequest request, CancellationToken ct = default);
    Task<ApiResponse<OrderDto>> GetOrderByIdAsync(OrderGetByIdRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> UpdateOrderStatusAsync(UpdateOrderStatusRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> CreateShipmentAsync(CreateOrderShipmentRequest request, CancellationToken ct = default);
}

public interface IAdminUserService
{
    Task<ApiResponse<PagedResult<AdminUserDto>>> GetUserListAsync(AdminUserListRequest request, CancellationToken ct = default);
    Task<ApiResponse<AdminUserDto>> GetUserByIdAsync(AdminUserGetByIdRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> CreateUserAsync(CreateAdminUserRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> UpdateUserAsync(UpdateAdminUserRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> UpdateUserStatusAsync(AdminUserStatusUpdateRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> ResetPasswordAsync(AdminUserPasswordResetRequest request, CancellationToken ct = default);
    Task<ApiResponse<List<LookupItem>>> GetRoleOptionsAsync(CancellationToken ct = default);
}

public interface IAdminInventoryService
{
    Task<ApiResponse<object>> StockInAsync(InventoryAdjustmentRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> StockOutAsync(InventoryAdjustmentRequest request, CancellationToken ct = default);
    Task<ApiResponse<PagedResult<InventoryTransactionDto>>> GetTransactionListAsync(ProductInventoryTransactionListRequest request, CancellationToken ct = default);
    Task<ApiResponse<List<InventoryLowStockDto>>> GetInventoryReportAsync(InventoryAdminReportRequest request, CancellationToken ct = default);
}

public interface IAdminAuditLogService
{
    Task<ApiResponse<PagedResult<AuditLogDto>>> GetAuditLogListAsync(AuditLogListRequest request, CancellationToken ct = default);
}

public interface IAdminInvoiceService
{
    Task<ApiResponse<PagedResult<InvoiceAdminDto>>> GetInvoiceListAsync(PaginationRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> GenerateInvoiceAsync(GenerateInvoiceRequest request, CancellationToken ct = default);
}

public interface IAdminPaymentService
{
    Task<ApiResponse<PagedResult<PaymentAdminDto>>> GetPaymentListAsync(PaginationRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> ProcessRefundAsync(ProcessPaymentRefundRequest request, CancellationToken ct = default);
}

public interface IAdminOperationsService
{
    Task<ApiResponse<object>> GetDataIntegrityCheckAsync(CancellationToken ct = default);
}
