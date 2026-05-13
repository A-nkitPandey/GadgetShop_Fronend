using GadgetShop.Models;

namespace GadgetShop.Application.Interfaces;

public interface IAuthService
{
    Task<ApiResponse<LoginResponse>> LoginAsync(LoginRequest request, CancellationToken ct = default);
    Task<ApiResponse<AuthTokenResponse>> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken ct = default);
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
    Task<ApiResponse<InvoicePrintDocumentDto>> GetMyInvoiceAsync(OrderGetByIdRequest request, CancellationToken ct = default);
    Task<ApiResponse<OrderShipmentDto>> GetMyShipmentByOrderIdAsync(OrderShipmentGetByOrderIdRequest request, CancellationToken ct = default);
    Task<ApiResponse<PagedResult<OrderReturnDto>>> GetMyReturnListAsync(MyReturnListRequest request, CancellationToken ct = default);
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
    Task<ApiResponse<object>> MarkNotificationAsReadAsync(long notificationId, CancellationToken ct = default);
    Task<ApiResponse<object>> RegisterDeviceAsync(RegisterPushDeviceRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> RemoveDeviceAsync(RemovePushDeviceRequest request, CancellationToken ct = default);
}

public interface ISupportTicketService
{
    Task<ApiResponse<List<SupportTicketDto>>> GetMyTicketsAsync(CancellationToken ct = default);
    Task<ApiResponse<SupportTicketDto>> CreateAsync(SupportTicketMutationRequest request, CancellationToken ct = default);
    Task<ApiResponse<SupportTicketDto>> ReplyAsync(SupportTicketReplyRequest request, CancellationToken ct = default);
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
    Task<ApiResponse<ProductImageUploadDto>> UploadProductImageAsync(ProductImageUploadRequest request, CancellationToken ct = default);
    Task<ApiResponse<ProductGalleryImageDto>> UploadProductGalleryImageAsync(ProductGalleryImageUploadRequest request, CancellationToken ct = default);
    Task<ApiResponse<List<ProductGalleryImageDto>>> GetProductGalleryAsync(ProductGalleryGetByProductIdRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> DeleteProductGalleryImageAsync(ProductGalleryImageDeleteRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> SetPrimaryProductGalleryImageAsync(ProductPrimaryImageUpdateRequest request, CancellationToken ct = default);
    Task<ApiResponse<GenerateProductContentResponseModel>> GenerateAIDescriptionAsync(GenerateProductContentRequestModel request, CancellationToken ct = default);
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
    Task<ApiResponse<object>> UpdateShipmentStatusAsync(UpdateOrderShipmentStatusRequest request, CancellationToken ct = default);
    Task<ApiResponse<PagedResult<OrderShipmentDto>>> GetShipmentListAsync(OrderShipmentListRequest request, CancellationToken ct = default);
    Task<ApiResponse<OrderShipmentDto>> GetShipmentByOrderIdAsync(OrderShipmentGetByOrderIdRequest request, CancellationToken ct = default);
    Task<ApiResponse<PagedResult<OrderReturnDto>>> GetReturnListAsync(OrderReturnListRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> UpdateReturnStatusAsync(UpdateOrderReturnStatusRequest request, CancellationToken ct = default);
}

public interface IAdminVariantService
{
    Task<ApiResponse<PagedResult<AdminProductVariantDto>>> GetVariantListAsync(ProductVariantListRequest request, CancellationToken ct = default);
    Task<ApiResponse<AdminProductVariantDto>> GetVariantByIdAsync(ProductVariantGetByIdRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> CreateVariantAsync(CreateProductVariantRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> UpdateVariantAsync(UpdateProductVariantRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> UpdateVariantStatusAsync(ProductVariantStatusUpdateRequest request, CancellationToken ct = default);
}

public interface IAdminAttributeService
{
    Task<ApiResponse<PagedResult<AttributeMasterDto>>> GetAttributeListAsync(AttributeMasterListRequest request, CancellationToken ct = default);
    Task<ApiResponse<AttributeMasterDto>> GetAttributeByIdAsync(AttributeMasterGetByIdRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> CreateAttributeAsync(CreateAttributeMasterRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> UpdateAttributeAsync(UpdateAttributeMasterRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> UpdateAttributeStatusAsync(AttributeMasterStatusUpdateRequest request, CancellationToken ct = default);
    Task<ApiResponse<PagedResult<AttributeValueDto>>> GetAttributeValueListAsync(AttributeValueListRequest request, CancellationToken ct = default);
    Task<ApiResponse<AttributeValueDto>> GetAttributeValueByIdAsync(AttributeValueGetByIdRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> CreateAttributeValueAsync(CreateAttributeValueRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> UpdateAttributeValueAsync(UpdateAttributeValueRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> UpdateAttributeValueStatusAsync(AttributeValueStatusUpdateRequest request, CancellationToken ct = default);
    Task<ApiResponse<List<VariantAttributeMappingDto>>> GetVariantAttributeMappingsAsync(VariantAttributeMappingGetByVariantIdRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> SaveVariantAttributeMappingsAsync(SaveVariantAttributeMappingRequest request, CancellationToken ct = default);
}

public interface IRolePermissionService
{
    Task<ApiResponse<PagedResult<RoleDto>>> GetRoleListAsync(RoleListRequest request, CancellationToken ct = default);
    Task<ApiResponse<RoleDto>> GetRoleByIdAsync(RoleGetByIdRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> CreateRoleAsync(CreateRoleRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> UpdateRoleAsync(UpdateRoleRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> UpdateRoleStatusAsync(RoleStatusUpdateRequest request, CancellationToken ct = default);
    Task<ApiResponse<List<PermissionOptionDto>>> GetPermissionOptionsAsync(CancellationToken ct = default);
}

public interface IAdminArchiveService
{
    Task<ApiResponse<RunArchiveDto>> RunArchiveAsync(RunArchiveRequest request, CancellationToken ct = default);
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
    Task<ApiResponse<InvoiceAdminDetailDto>> GetInvoiceByIdAsync(InvoiceGetByIdRequest request, CancellationToken ct = default);
    Task<ApiResponse<InvoicePrintDocumentDto>> GetInvoicePrintDocumentAsync(InvoicePrintDocumentRequest request, CancellationToken ct = default);
}

public interface IAdminPaymentService
{
    Task<ApiResponse<PagedResult<PaymentAdminDto>>> GetPaymentListAsync(PaginationRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> ProcessRefundAsync(ProcessPaymentRefundRequest request, CancellationToken ct = default);
}

public interface IAdminOperationsService
{
    Task<ApiResponse<AdminOperationResultDto>> ReleaseExpiredReservationsAsync(ReleaseExpiredCartReservationsRequest request, CancellationToken ct = default);
    Task<ApiResponse<AdminOperationResultDto>> RecalculateOrderTotalsAsync(RecalculateOrderTotalsRequest request, CancellationToken ct = default);
    Task<ApiResponse<AdminDataIntegrityCheckDto>> GetDataIntegrityCheckAsync(CancellationToken ct = default);
}
