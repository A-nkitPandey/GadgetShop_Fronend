using GadgetShop.ApiClients;
using GadgetShop.Authentication;
using GadgetShop.Constants;
using GadgetShop.Models;

namespace GadgetShop.Services;

// ─── Auth Service ─────────────────────────────────────────────
public sealed class AuthService(IApiClient api, TokenStorageService tokenStorage, AuthStateProvider authState)
{
    public async Task<ApiResponse<LoginResponse>> LoginAsync(LoginRequest request, CancellationToken ct = default)
    {
        var response = await api.PostAsync<LoginRequest, LoginResponse>(ApiEndpoints.Auth.Login, request, ct);
        if (response.IsSuccess && response.Data?.Token is { } token)
        {
            await tokenStorage.SetAccessTokenAsync(token);
            await tokenStorage.SetUserAsync(response.Data);
            authState.NotifyAuthChanged();
        }
        return response;
    }

    public async Task<ApiResponse<object>> RegisterAsync(CustomerRegisterRequest request, CancellationToken ct = default)
        => await api.PostAsync<CustomerRegisterRequest, object>(ApiEndpoints.CustomerAccount.Register, request, ct);

    public async Task LogoutAsync()
    {
        await tokenStorage.ClearAsync();
        authState.NotifyAuthChanged();
    }

    public async Task<bool> IsAuthenticatedAsync()
    {
        var token = await tokenStorage.GetAccessTokenAsync();
        return !string.IsNullOrWhiteSpace(token);
    }

    public async Task<bool> IsAdminAsync()
    {
        var roles = await authState.GetRolesAsync();
        return roles.Contains("Admin") || roles.Contains("SuperAdmin");
    }
}

// ─── Catalog Service ──────────────────────────────────────────
public sealed class CatalogService(IApiClient api)
{
    public Task<ApiResponse<PagedResult<CustomerCatalogProductDto>>> GetProductsAsync(
        CustomerCatalogListRequest request, CancellationToken ct = default)
        => api.PostAsync<CustomerCatalogListRequest, PagedResult<CustomerCatalogProductDto>>(
            ApiEndpoints.CustomerCatalog.GetProductList, request, ct);

    public Task<ApiResponse<CustomerCatalogProductDto>> GetProductByIdAsync(
        CustomerCatalogGetByIdRequest request, CancellationToken ct = default)
        => api.PostAsync<CustomerCatalogGetByIdRequest, CustomerCatalogProductDto>(
            ApiEndpoints.CustomerCatalog.GetProductById, request, ct);

    public Task<ApiResponse<PagedResult<ProductReviewDto>>> GetReviewsAsync(
        ProductReviewListRequest request, CancellationToken ct = default)
        => api.PostAsync<ProductReviewListRequest, PagedResult<ProductReviewDto>>(
            ApiEndpoints.ProductReview.GetByProduct, request, ct);

    public Task<ApiResponse<object>> SaveReviewAsync(ProductReviewMutationRequest request, CancellationToken ct = default)
        => api.PostAsync<ProductReviewMutationRequest, object>(ApiEndpoints.ProductReview.Save, request, ct);

    public Task<ApiResponse<object>> DeleteReviewAsync(ProductReviewDeleteRequest request, CancellationToken ct = default)
        => api.PostAsync<ProductReviewDeleteRequest, object>(ApiEndpoints.ProductReview.Delete, request, ct);
}

// ─── Cart Service ─────────────────────────────────────────────
public sealed class CartService(IApiClient api)
{
    public Task<ApiResponse<CartDto>> GetMyCartAsync(CancellationToken ct = default)
        => api.GetAsync<CartDto>(ApiEndpoints.Cart.GetMyCart, ct);

    public Task<ApiResponse<CartDto>> AddItemAsync(AddCartItemRequest request, CancellationToken ct = default)
        => api.PostAsync<AddCartItemRequest, CartDto>(ApiEndpoints.Cart.AddItem, request, ct);

    public Task<ApiResponse<CartDto>> UpdateQuantityAsync(UpdateCartItemQuantityRequest request, CancellationToken ct = default)
        => api.PostAsync<UpdateCartItemQuantityRequest, CartDto>(ApiEndpoints.Cart.UpdateItemQuantity, request, ct);

    public Task<ApiResponse<CartDto>> RemoveItemAsync(RemoveCartItemRequest request, CancellationToken ct = default)
        => api.PostAsync<RemoveCartItemRequest, CartDto>(ApiEndpoints.Cart.RemoveItem, request, ct);

    public Task<ApiResponse<object>> ClearCartAsync(CancellationToken ct = default)
        => api.PostEmptyAsync<object>(ApiEndpoints.Cart.ClearCart, ct);

    public Task<ApiResponse<CartDto>> ApplyCouponAsync(ApplyCartCouponRequest request, CancellationToken ct = default)
        => api.PostAsync<ApplyCartCouponRequest, CartDto>(ApiEndpoints.Cart.ApplyCoupon, request, ct);

    public Task<ApiResponse<CartDto>> RemoveCouponAsync(CancellationToken ct = default)
        => api.PostEmptyAsync<CartDto>(ApiEndpoints.Cart.RemoveCoupon, ct);
}

// ─── Order Service ────────────────────────────────────────────
public sealed class OrderService(IApiClient api)
{
    public Task<ApiResponse<OrderDto>> PlaceOrderAsync(PlaceOrderRequest request, CancellationToken ct = default)
        => api.PostAsync<PlaceOrderRequest, OrderDto>(ApiEndpoints.Order.PlaceOrder, request, ct);

    public Task<ApiResponse<OrderDto>> GetMyOrderByIdAsync(OrderGetByIdRequest request, CancellationToken ct = default)
        => api.PostAsync<OrderGetByIdRequest, OrderDto>(ApiEndpoints.Order.GetMyOrderById, request, ct);

    public Task<ApiResponse<PagedResult<OrderDto>>> GetMyOrderListAsync(OrderListRequest request, CancellationToken ct = default)
        => api.PostAsync<OrderListRequest, PagedResult<OrderDto>>(ApiEndpoints.Order.GetMyOrderList, request, ct);

    public Task<ApiResponse<object>> CancelMyOrderAsync(CancelMyOrderRequest request, CancellationToken ct = default)
        => api.PostAsync<CancelMyOrderRequest, object>(ApiEndpoints.Order.CancelMyOrder, request, ct);

    public Task<ApiResponse<object>> ReorderAsync(ReorderRequest request, CancellationToken ct = default)
        => api.PostAsync<ReorderRequest, object>(ApiEndpoints.Order.Reorder, request, ct);

    public Task<ApiResponse<object>> CreateReturnAsync(CreateOrderReturnRequest request, CancellationToken ct = default)
        => api.PostAsync<CreateOrderReturnRequest, object>(ApiEndpoints.Order.CreateReturn, request, ct);
}

// ─── Wishlist Service ─────────────────────────────────────────
public sealed class WishlistService(IApiClient api)
{
    public Task<ApiResponse<List<WishlistItemDto>>> GetMyWishlistAsync(CancellationToken ct = default)
        => api.GetAsync<List<WishlistItemDto>>(ApiEndpoints.Wishlist.GetMyWishlist, ct);

    public Task<ApiResponse<object>> AddAsync(WishlistMutationRequest request, CancellationToken ct = default)
        => api.PostAsync<WishlistMutationRequest, object>(ApiEndpoints.Wishlist.Add, request, ct);

    public Task<ApiResponse<object>> RemoveAsync(WishlistRemoveRequest request, CancellationToken ct = default)
        => api.PostAsync<WishlistRemoveRequest, object>(ApiEndpoints.Wishlist.Remove, request, ct);

    public Task<ApiResponse<object>> MoveToCartAsync(WishlistRemoveRequest request, CancellationToken ct = default)
        => api.PostAsync<WishlistRemoveRequest, object>(ApiEndpoints.Wishlist.MoveToCart, request, ct);
}

// ─── User Account Service ─────────────────────────────────────
public sealed class UserAccountService(IApiClient api)
{
    public Task<ApiResponse<CustomerProfileDto>> GetMyProfileAsync(CancellationToken ct = default)
        => api.GetAsync<CustomerProfileDto>(ApiEndpoints.CustomerAccount.GetMyProfile, ct);

    public Task<ApiResponse<object>> UpdateMyProfileAsync(CustomerProfileUpdateRequest request, CancellationToken ct = default)
        => api.PostAsync<CustomerProfileUpdateRequest, object>(ApiEndpoints.CustomerAccount.UpdateMyProfile, request, ct);

    public Task<ApiResponse<object>> ChangePasswordAsync(ChangePasswordRequest request, CancellationToken ct = default)
        => api.PostAsync<ChangePasswordRequest, object>(ApiEndpoints.CustomerAccount.ChangePassword, request, ct);

    public Task<ApiResponse<List<CustomerAddressDto>>> GetMyAddressesAsync(CancellationToken ct = default)
        => api.GetAsync<List<CustomerAddressDto>>(ApiEndpoints.CustomerAddress.GetMyAddresses, ct);

    public Task<ApiResponse<object>> SaveAddressAsync(CustomerAddressMutationRequest request, CancellationToken ct = default)
        => api.PostAsync<CustomerAddressMutationRequest, object>(ApiEndpoints.CustomerAddress.SaveAddress, request, ct);

    public Task<ApiResponse<object>> DeleteAddressAsync(CustomerAddressDeleteRequest request, CancellationToken ct = default)
        => api.PostAsync<CustomerAddressDeleteRequest, object>(ApiEndpoints.CustomerAddress.DeleteAddress, request, ct);

    public Task<ApiResponse<List<NotificationDto>>> GetMyNotificationsAsync(CancellationToken ct = default)
        => api.GetAsync<List<NotificationDto>>(ApiEndpoints.Notification.GetMyNotifications, ct);
}

// ─── Payment Service ──────────────────────────────────────────
public sealed class PaymentService(IApiClient api)
{
    public Task<ApiResponse<PaymentOrderDto>> CreatePaymentOrderAsync(CreatePaymentOrderRequest request, CancellationToken ct = default)
        => api.PostAsync<CreatePaymentOrderRequest, PaymentOrderDto>(ApiEndpoints.Payment.CreatePaymentOrder, request, ct);

    public Task<ApiResponse<object>> VerifyPaymentAsync(VerifyPaymentRequest request, CancellationToken ct = default)
        => api.PostAsync<VerifyPaymentRequest, object>(ApiEndpoints.Payment.VerifyPayment, request, ct);

    public Task<ApiResponse<object>> RetryPaymentAsync(RetryPaymentRequest request, CancellationToken ct = default)
        => api.PostAsync<RetryPaymentRequest, object>(ApiEndpoints.Payment.RetryPayment, request, ct);
}

// ─── Admin Dashboard Service ──────────────────────────────────
public sealed class AdminDashboardService(IApiClient api)
{
    public Task<ApiResponse<AdminDashboardSummaryDto>> GetSummaryAsync(CancellationToken ct = default)
        => api.GetAsync<AdminDashboardSummaryDto>(ApiEndpoints.AdminDashboard.GetSummary, ct);

    public Task<ApiResponse<AdminDashboardOverviewDto>> GetOverviewAsync(CancellationToken ct = default)
        => api.GetAsync<AdminDashboardOverviewDto>(ApiEndpoints.AdminDashboard.GetDashboardOverview, ct);

    public Task<ApiResponse<object>> GetReportAsync(AdminDashboardReportRequest request, CancellationToken ct = default)
        => api.PostAsync<AdminDashboardReportRequest, object>(ApiEndpoints.AdminDashboard.GetDashboardReport, request, ct);
}

// ─── Admin Product Service ────────────────────────────────────
public sealed class AdminProductService(IApiClient api)
{
    public Task<ApiResponse<PagedResult<AdminProductDto>>> GetProductListAsync(PaginationRequest request, CancellationToken ct = default)
        => api.PostAsync<PaginationRequest, PagedResult<AdminProductDto>>(ApiEndpoints.ProductMaster.GetProductList, request, ct);

    public Task<ApiResponse<AdminProductDto>> GetProductByIdAsync(ProductGetByIdRequest request, CancellationToken ct = default)
        => api.PostAsync<ProductGetByIdRequest, AdminProductDto>(ApiEndpoints.ProductMaster.GetProductById, request, ct);

    public Task<ApiResponse<object>> CreateProductAsync(CreateProductRequest request, CancellationToken ct = default)
        => api.PostAsync<CreateProductRequest, object>(ApiEndpoints.ProductMaster.CreateProduct, request, ct);

    public Task<ApiResponse<object>> UpdateProductAsync(UpdateProductRequest request, CancellationToken ct = default)
        => api.PostAsync<UpdateProductRequest, object>(ApiEndpoints.ProductMaster.UpdateProduct, request, ct);

    public Task<ApiResponse<object>> UpdateProductStatusAsync(ProductStatusUpdateRequest request, CancellationToken ct = default)
        => api.PostAsync<ProductStatusUpdateRequest, object>(ApiEndpoints.ProductMaster.UpdateProductStatus, request, ct);

    public Task<ApiResponse<PagedResult<CategoryDto>>> GetCategoryListAsync(PaginationRequest request, CancellationToken ct = default)
        => api.PostAsync<PaginationRequest, PagedResult<CategoryDto>>(ApiEndpoints.CategoryMaster.GetCategoryList, request, ct);

    public Task<ApiResponse<object>> CreateCategoryAsync(CreateCategoryRequest request, CancellationToken ct = default)
        => api.PostAsync<CreateCategoryRequest, object>(ApiEndpoints.CategoryMaster.CreateCategory, request, ct);

    public Task<ApiResponse<object>> UpdateCategoryAsync(UpdateCategoryRequest request, CancellationToken ct = default)
        => api.PostAsync<UpdateCategoryRequest, object>(ApiEndpoints.CategoryMaster.UpdateCategory, request, ct);

    public Task<ApiResponse<object>> UpdateCategoryStatusAsync(CategoryStatusUpdateRequest request, CancellationToken ct = default)
        => api.PostAsync<CategoryStatusUpdateRequest, object>(ApiEndpoints.CategoryMaster.UpdateCategoryStatus, request, ct);

    public Task<ApiResponse<PagedResult<BrandDto>>> GetBrandListAsync(PaginationRequest request, CancellationToken ct = default)
        => api.PostAsync<PaginationRequest, PagedResult<BrandDto>>(ApiEndpoints.BrandMaster.GetBrandList, request, ct);

    public Task<ApiResponse<object>> CreateBrandAsync(CreateBrandRequest request, CancellationToken ct = default)
        => api.PostAsync<CreateBrandRequest, object>(ApiEndpoints.BrandMaster.CreateBrand, request, ct);

    public Task<ApiResponse<object>> UpdateBrandAsync(UpdateBrandRequest request, CancellationToken ct = default)
        => api.PostAsync<UpdateBrandRequest, object>(ApiEndpoints.BrandMaster.UpdateBrand, request, ct);

    public Task<ApiResponse<object>> UpdateBrandStatusAsync(BrandStatusUpdateRequest request, CancellationToken ct = default)
        => api.PostAsync<BrandStatusUpdateRequest, object>(ApiEndpoints.BrandMaster.UpdateBrandStatus, request, ct);

    public Task<ApiResponse<PagedResult<CouponDto>>> GetCouponListAsync(PaginationRequest request, CancellationToken ct = default)
        => api.PostAsync<PaginationRequest, PagedResult<CouponDto>>(ApiEndpoints.CouponMaster.GetCouponList, request, ct);

    public Task<ApiResponse<object>> CreateCouponAsync(CreateCouponRequest request, CancellationToken ct = default)
        => api.PostAsync<CreateCouponRequest, object>(ApiEndpoints.CouponMaster.CreateCoupon, request, ct);

    public Task<ApiResponse<object>> UpdateCouponAsync(UpdateCouponRequest request, CancellationToken ct = default)
        => api.PostAsync<UpdateCouponRequest, object>(ApiEndpoints.CouponMaster.UpdateCoupon, request, ct);

    public Task<ApiResponse<object>> UpdateCouponStatusAsync(CouponStatusUpdateRequest request, CancellationToken ct = default)
        => api.PostAsync<CouponStatusUpdateRequest, object>(ApiEndpoints.CouponMaster.UpdateCouponStatus, request, ct);
}

// ─── Admin Order Service ──────────────────────────────────────
public sealed class AdminOrderService(IApiClient api)
{
    public Task<ApiResponse<PagedResult<OrderDto>>> GetOrderListAsync(OrderListRequest request, CancellationToken ct = default)
        => api.PostAsync<OrderListRequest, PagedResult<OrderDto>>(ApiEndpoints.Order.GetOrderList, request, ct);

    public Task<ApiResponse<OrderDto>> GetOrderByIdAsync(OrderGetByIdRequest request, CancellationToken ct = default)
        => api.PostAsync<OrderGetByIdRequest, OrderDto>(ApiEndpoints.Order.GetOrderById, request, ct);

    public Task<ApiResponse<object>> UpdateOrderStatusAsync(UpdateOrderStatusRequest request, CancellationToken ct = default)
        => api.PostAsync<UpdateOrderStatusRequest, object>(ApiEndpoints.Order.UpdateOrderStatus, request, ct);

    public Task<ApiResponse<object>> CreateShipmentAsync(CreateOrderShipmentRequest request, CancellationToken ct = default)
        => api.PostAsync<CreateOrderShipmentRequest, object>(ApiEndpoints.Order.CreateShipment, request, ct);
}

// ─── Admin User Service ───────────────────────────────────────
public sealed class AdminUserService(IApiClient api)
{
    public Task<ApiResponse<PagedResult<AdminUserDto>>> GetUserListAsync(AdminUserListRequest request, CancellationToken ct = default)
        => api.PostAsync<AdminUserListRequest, PagedResult<AdminUserDto>>(ApiEndpoints.AdminUser.GetUserList, request, ct);

    public Task<ApiResponse<AdminUserDto>> GetUserByIdAsync(AdminUserGetByIdRequest request, CancellationToken ct = default)
        => api.PostAsync<AdminUserGetByIdRequest, AdminUserDto>(ApiEndpoints.AdminUser.GetUserById, request, ct);

    public Task<ApiResponse<object>> CreateUserAsync(CreateAdminUserRequest request, CancellationToken ct = default)
        => api.PostAsync<CreateAdminUserRequest, object>(ApiEndpoints.AdminUser.CreateUser, request, ct);

    public Task<ApiResponse<object>> UpdateUserAsync(UpdateAdminUserRequest request, CancellationToken ct = default)
        => api.PostAsync<UpdateAdminUserRequest, object>(ApiEndpoints.AdminUser.UpdateUser, request, ct);

    public Task<ApiResponse<object>> UpdateUserStatusAsync(AdminUserStatusUpdateRequest request, CancellationToken ct = default)
        => api.PostAsync<AdminUserStatusUpdateRequest, object>(ApiEndpoints.AdminUser.UpdateUserStatus, request, ct);

    public Task<ApiResponse<object>> ResetPasswordAsync(AdminUserPasswordResetRequest request, CancellationToken ct = default)
        => api.PostAsync<AdminUserPasswordResetRequest, object>(ApiEndpoints.AdminUser.ResetPassword, request, ct);

    public Task<ApiResponse<List<LookupItem>>> GetRoleOptionsAsync(CancellationToken ct = default)
        => api.GetAsync<List<LookupItem>>(ApiEndpoints.AdminUser.GetRoleOptions, ct);
}

// ─── Admin Inventory Service ──────────────────────────────────
public sealed class AdminInventoryService(IApiClient api)
{
    public Task<ApiResponse<object>> StockInAsync(InventoryAdjustmentRequest request, CancellationToken ct = default)
        => api.PostAsync<InventoryAdjustmentRequest, object>(ApiEndpoints.ProductInventory.StockIn, request, ct);

    public Task<ApiResponse<object>> StockOutAsync(InventoryAdjustmentRequest request, CancellationToken ct = default)
        => api.PostAsync<InventoryAdjustmentRequest, object>(ApiEndpoints.ProductInventory.StockOut, request, ct);

    public Task<ApiResponse<PagedResult<object>>> GetTransactionListAsync(ProductInventoryTransactionListRequest request, CancellationToken ct = default)
        => api.PostAsync<ProductInventoryTransactionListRequest, PagedResult<object>>(ApiEndpoints.ProductInventory.GetTransactionList, request, ct);

    public Task<ApiResponse<object>> GetInventoryReportAsync(InventoryAdminReportRequest request, CancellationToken ct = default)
        => api.PostAsync<InventoryAdminReportRequest, object>(ApiEndpoints.ProductInventory.GetInventoryAdminReport, request, ct);
}

// ─── Admin Catalog Admin Service (alias, keeps naming clean) ──
public sealed class AdminCatalogAdminService(IApiClient api)
{
    // Audit log
    public Task<ApiResponse<PagedResult<object>>> GetAuditLogListAsync(AuditLogListRequest request, CancellationToken ct = default)
        => api.PostAsync<AuditLogListRequest, PagedResult<object>>(ApiEndpoints.AuditLog.GetAuditLogList, request, ct);

    // Data integrity
    public Task<ApiResponse<object>> GetDataIntegrityCheckAsync(CancellationToken ct = default)
        => api.GetAsync<object>(ApiEndpoints.AdminOperations.GetDataIntegrityCheck, ct);

    // Invoice
    public Task<ApiResponse<object>> GetInvoiceListAsync(PaginationRequest request, CancellationToken ct = default)
        => api.PostAsync<PaginationRequest, object>(ApiEndpoints.Invoice.GetInvoiceList, request, ct);

    public Task<ApiResponse<object>> GenerateInvoiceAsync(GenerateInvoiceRequest request, CancellationToken ct = default)
        => api.PostAsync<GenerateInvoiceRequest, object>(ApiEndpoints.Invoice.GenerateInvoice, request, ct);

    // Payment admin
    public Task<ApiResponse<PagedResult<object>>> GetPaymentListAsync(PaginationRequest request, CancellationToken ct = default)
        => api.PostAsync<PaginationRequest, PagedResult<object>>(ApiEndpoints.PaymentAdmin.GetPaymentList, request, ct);

    public Task<ApiResponse<object>> ProcessRefundAsync(ProcessPaymentRefundRequest request, CancellationToken ct = default)
        => api.PostAsync<ProcessPaymentRefundRequest, object>(ApiEndpoints.PaymentAdmin.ProcessRefund, request, ct);
}
