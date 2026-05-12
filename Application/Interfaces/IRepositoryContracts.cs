using GadgetShop.Models;

namespace GadgetShop.Application.Interfaces;

public interface IAuthRepository
{
    Task<ApiResponse<LoginResponse>> LoginAsync(LoginRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> RegisterAsync(CustomerRegisterRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> ForgotPasswordAsync(ForgotPasswordRequest request, CancellationToken ct = default);
}

public interface ICatalogRepository
{
    Task<ApiResponse<BackendPaginationResponse<List<BackendCustomerCatalogListItem>>>> GetProductsAsync(CustomerCatalogListRequest request, CancellationToken ct = default);
    Task<ApiResponse<BackendProductDetail>> GetProductByIdAsync(CustomerCatalogGetByIdRequest request, CancellationToken ct = default);
    Task<ApiResponse<BackendRecommendationResponse>> GetRecommendationsAsync(BackendRecommendationRequest request, CancellationToken ct = default);
    Task<ApiResponse<BackendPaginationResponse<List<BackendProductReview>>>> GetReviewsAsync(ProductReviewListRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> SaveReviewAsync(ProductReviewMutationRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> DeleteReviewAsync(ProductReviewDeleteRequest request, CancellationToken ct = default);
}

public interface ICartRepository
{
    Task<ApiResponse<BackendCart>> GetMyCartAsync(CancellationToken ct = default);
    Task<ApiResponse<BackendCartMutation>> AddItemAsync(AddCartItemRequest request, CancellationToken ct = default);
    Task<ApiResponse<BackendCartMutation>> UpdateQuantityAsync(UpdateCartItemQuantityRequest request, CancellationToken ct = default);
    Task<ApiResponse<BackendCartMutation>> RemoveItemAsync(RemoveCartItemRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> ClearCartAsync(CancellationToken ct = default);
    Task<ApiResponse<BackendCart>> ApplyCouponAsync(ApplyCartCouponRequest request, CancellationToken ct = default);
    Task<ApiResponse<BackendCart>> RemoveCouponAsync(CancellationToken ct = default);
}

public interface IWishlistRepository
{
    Task<ApiResponse<List<BackendWishlistItem>>> GetMyWishlistAsync(CancellationToken ct = default);
    Task<ApiResponse<object>> AddAsync(WishlistMutationRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> RemoveAsync(WishlistRemoveRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> MoveToCartAsync(WishlistRemoveRequest request, CancellationToken ct = default);
}

public interface IOrderRepository
{
    Task<ApiResponse<BackendPlaceOrderResponse>> PlaceOrderAsync(PlaceOrderRequest request, CancellationToken ct = default);
    Task<ApiResponse<BackendOrderDetail>> GetMyOrderByIdAsync(OrderGetByIdRequest request, CancellationToken ct = default);
    Task<ApiResponse<BackendPaginationResponse<List<BackendOrderGrid>>>> GetMyOrderListAsync(OrderListRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> CancelMyOrderAsync(CancelMyOrderRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> ReorderAsync(ReorderRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> CreateReturnAsync(CreateOrderReturnRequest request, CancellationToken ct = default);
    Task<ApiResponse<BackendPaginationResponse<List<BackendOrderGrid>>>> GetOrderListAsync(OrderListRequest request, CancellationToken ct = default);
    Task<ApiResponse<BackendOrderDetail>> GetOrderByIdAsync(OrderGetByIdRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> UpdateOrderStatusAsync(UpdateOrderStatusRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> CreateShipmentAsync(CreateOrderShipmentRequest request, CancellationToken ct = default);
}

public interface IUserAccountRepository
{
    Task<ApiResponse<CustomerProfileDto>> GetMyProfileAsync(CancellationToken ct = default);
    Task<ApiResponse<object>> UpdateMyProfileAsync(CustomerProfileUpdateRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> ChangePasswordAsync(ChangePasswordRequest request, CancellationToken ct = default);
    Task<ApiResponse<List<BackendAddress>>> GetMyAddressesAsync(CancellationToken ct = default);
    Task<ApiResponse<object>> SaveAddressAsync(BackendAddressRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> DeleteAddressAsync(CustomerAddressDeleteRequest request, CancellationToken ct = default);
    Task<ApiResponse<List<NotificationDto>>> GetMyNotificationsAsync(CancellationToken ct = default);
}

public interface IPaymentRepository
{
    Task<ApiResponse<BackendPaymentOrder>> CreatePaymentOrderAsync(CreatePaymentOrderRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> VerifyPaymentAsync(VerifyPaymentRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> RetryPaymentAsync(RetryPaymentRequest request, CancellationToken ct = default);
}

public interface IAdminDashboardRepository
{
    Task<ApiResponse<BackendAdminDashboardSummary>> GetSummaryAsync(CancellationToken ct = default);
    Task<ApiResponse<BackendAdminDashboardOverview>> GetOverviewAsync(CancellationToken ct = default);
    Task<ApiResponse<object>> GetReportAsync(AdminDashboardReportRequest request, CancellationToken ct = default);
}

public interface IAdminProductRepository
{
    Task<ApiResponse<BackendPaginationResponse<List<BackendProductGrid>>>> GetProductListAsync(PaginationRequest request, CancellationToken ct = default);
    Task<ApiResponse<AdminProductDto>> GetProductByIdAsync(ProductGetByIdRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> CreateProductAsync(CreateProductRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> UpdateProductAsync(UpdateProductRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> UpdateProductStatusAsync(ProductStatusUpdateRequest request, CancellationToken ct = default);
    Task<ApiResponse<BackendPaginationResponse<List<BackendCategoryGrid>>>> GetCategoryListAsync(PaginationRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> CreateCategoryAsync(CreateCategoryRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> UpdateCategoryAsync(UpdateCategoryRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> UpdateCategoryStatusAsync(CategoryStatusUpdateRequest request, CancellationToken ct = default);
    Task<ApiResponse<BackendPaginationResponse<List<BackendBrandGrid>>>> GetBrandListAsync(PaginationRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> CreateBrandAsync(CreateBrandRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> UpdateBrandAsync(UpdateBrandRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> UpdateBrandStatusAsync(BrandStatusUpdateRequest request, CancellationToken ct = default);
    Task<ApiResponse<BackendPaginationResponse<List<BackendCouponGrid>>>> GetCouponListAsync(PaginationRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> CreateCouponAsync(CreateCouponRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> UpdateCouponAsync(UpdateCouponRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> UpdateCouponStatusAsync(CouponStatusUpdateRequest request, CancellationToken ct = default);
}

public interface IAdminUserRepository
{
    Task<ApiResponse<BackendPaginationResponse<List<BackendAdminUserGrid>>>> GetUserListAsync(AdminUserListRequest request, CancellationToken ct = default);
    Task<ApiResponse<BackendAdminUserGrid>> GetUserByIdAsync(AdminUserGetByIdRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> CreateUserAsync(CreateAdminUserRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> UpdateUserAsync(UpdateAdminUserRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> UpdateUserStatusAsync(AdminUserStatusUpdateRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> ResetPasswordAsync(AdminUserPasswordResetRequest request, CancellationToken ct = default);
    Task<ApiResponse<List<LookupItem>>> GetRoleOptionsAsync(CancellationToken ct = default);
}

public interface IAdminInventoryRepository
{
    Task<ApiResponse<object>> StockInAsync(InventoryAdjustmentRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> StockOutAsync(InventoryAdjustmentRequest request, CancellationToken ct = default);
    Task<ApiResponse<BackendPaginationResponse<List<BackendInventoryTransaction>>>> GetTransactionListAsync(ProductInventoryTransactionListRequest request, CancellationToken ct = default);
    Task<ApiResponse<BackendInventoryReport>> GetInventoryReportAsync(InventoryAdminReportRequest request, CancellationToken ct = default);
}

public interface IAdminOperationsRepository
{
    Task<ApiResponse<BackendPaginationResponse<List<BackendAuditLogGrid>>>> GetAuditLogListAsync(AuditLogListRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> GetDataIntegrityCheckAsync(CancellationToken ct = default);
    Task<ApiResponse<BackendPaginationResponse<List<BackendInvoiceAdminGrid>>>> GetInvoiceListAsync(PaginationRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> GenerateInvoiceAsync(GenerateInvoiceRequest request, CancellationToken ct = default);
    Task<ApiResponse<BackendPaginationResponse<List<BackendPaymentAdminGrid>>>> GetPaymentListAsync(PaginationRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> ProcessRefundAsync(ProcessPaymentRefundRequest request, CancellationToken ct = default);
}
