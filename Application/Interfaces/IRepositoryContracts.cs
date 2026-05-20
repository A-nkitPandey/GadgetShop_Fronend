using GadgetShop.Models;

namespace GadgetShop.Application.Interfaces;

public interface IAuthRepository
{
    Task<ApiResponse<LoginResponse>> LoginAsync(LoginRequest request, CancellationToken ct = default);
    Task<ApiResponse<AuthTokenResponse>> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken ct = default);
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
    Task<ApiResponse<BackendInvoicePrintDocument>> GetMyInvoiceAsync(OrderGetByIdRequest request, CancellationToken ct = default);
    Task<ApiResponse<BackendOrderShipmentDetail>> GetMyShipmentByOrderIdAsync(OrderShipmentGetByOrderIdRequest request, CancellationToken ct = default);
    Task<ApiResponse<BackendPaginationResponse<List<BackendOrderReturnGrid>>>> GetMyReturnListAsync(MyReturnListRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> CreateReturnAsync(CreateOrderReturnRequest request, CancellationToken ct = default);
    Task<ApiResponse<BackendPaginationResponse<List<BackendOrderGrid>>>> GetOrderListAsync(OrderListRequest request, CancellationToken ct = default);
    Task<ApiResponse<BackendOrderDetail>> GetOrderByIdAsync(OrderGetByIdRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> UpdateOrderStatusAsync(UpdateOrderStatusRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> CreateShipmentAsync(CreateOrderShipmentRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> UpdateShipmentStatusAsync(UpdateOrderShipmentStatusRequest request, CancellationToken ct = default);
    Task<ApiResponse<BackendOrderShipmentDetail>> GetShipmentByOrderIdAsync(OrderShipmentGetByOrderIdRequest request, CancellationToken ct = default);
    Task<ApiResponse<BackendPaginationResponse<List<BackendOrderShipmentDetail>>>> GetShipmentListAsync(OrderShipmentListRequest request, CancellationToken ct = default);
    Task<ApiResponse<BackendPaginationResponse<List<BackendOrderReturnGrid>>>> GetReturnListAsync(OrderReturnListRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> UpdateReturnStatusAsync(UpdateOrderReturnStatusRequest request, CancellationToken ct = default);
}

public interface IUserAccountRepository
{
    Task<ApiResponse<CustomerProfileDto>> GetMyProfileAsync(CancellationToken ct = default);
    Task<ApiResponse<object>> UpdateMyProfileAsync(CustomerProfileUpdateRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> ChangePasswordAsync(ChangePasswordRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> DeactivateMyAccountAsync(CancellationToken ct = default);
    Task<ApiResponse<List<BackendAddress>>> GetMyAddressesAsync(CancellationToken ct = default);
    Task<ApiResponse<object>> SaveAddressAsync(BackendAddressRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> DeleteAddressAsync(CustomerAddressDeleteRequest request, CancellationToken ct = default);
    Task<ApiResponse<List<NotificationDto>>> GetMyNotificationsAsync(CancellationToken ct = default);
    Task<ApiResponse<object>> MarkNotificationAsReadAsync(long notificationId, CancellationToken ct = default);
    Task<ApiResponse<object>> RegisterDeviceAsync(RegisterPushDeviceRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> RemoveDeviceAsync(RemovePushDeviceRequest request, CancellationToken ct = default);
}

public interface ISupportTicketRepository
{
    Task<ApiResponse<List<SupportTicketDto>>> GetMyTicketsAsync(CancellationToken ct = default);
    Task<ApiResponse<SupportTicketDto>> CreateAsync(SupportTicketMutationRequest request, CancellationToken ct = default);
    Task<ApiResponse<SupportTicketDto>> ReplyAsync(SupportTicketReplyRequest request, CancellationToken ct = default);
    Task<ApiResponse<BackendPaginationResponse<List<SupportTicketDto>>>> GetAdminTicketsAsync(AdminSupportTicketListRequest request, CancellationToken ct = default);
    Task<ApiResponse<SupportTicketDto>> ReplyAsAdminAsync(AdminSupportTicketReplyRequest request, CancellationToken ct = default);
}

public interface IPaymentRepository
{
    Task<ApiResponse<BackendPaymentOrder>> CreatePaymentOrderAsync(CreatePaymentOrderRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> VerifyPaymentAsync(VerifyPaymentRequest request, CancellationToken ct = default);
    Task<ApiResponse<BackendPaymentOrder>> RetryPaymentAsync(RetryPaymentRequest request, CancellationToken ct = default);
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
    Task<ApiResponse<BackendProductDetail>> GetProductByIdAsync(ProductGetByIdRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> CreateProductAsync(CreateProductRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> UpdateProductAsync(UpdateProductRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> UpdateProductStatusAsync(ProductStatusUpdateRequest request, CancellationToken ct = default);
    Task<ApiResponse<ProductImageUploadDto>> UploadProductImageAsync(ProductImageUploadRequest request, CancellationToken ct = default);
    Task<ApiResponse<ProductGalleryImageDto>> UploadProductGalleryImageAsync(ProductGalleryImageUploadRequest request, CancellationToken ct = default);
    Task<ApiResponse<List<ProductGalleryImageDto>>> GetProductGalleryAsync(ProductGalleryGetByProductIdRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> DeleteProductGalleryImageAsync(ProductGalleryImageDeleteRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> SetPrimaryProductGalleryImageAsync(ProductPrimaryImageUpdateRequest request, CancellationToken ct = default);
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
    Task<ApiResponse<GenerateProductContentResponseModel>> GenerateAIDescriptionAsync(GenerateProductContentRequestModel request, CancellationToken ct = default);
}

public interface IAdminVariantRepository
{
    Task<ApiResponse<BackendPaginationResponse<List<BackendProductVariantGrid>>>> GetVariantListAsync(ProductVariantListRequest request, CancellationToken ct = default);
    Task<ApiResponse<BackendProductVariantDetail>> GetVariantByIdAsync(ProductVariantGetByIdRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> CreateVariantAsync(CreateProductVariantRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> UpdateVariantAsync(UpdateProductVariantRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> UpdateVariantStatusAsync(ProductVariantStatusUpdateRequest request, CancellationToken ct = default);
}

public interface IAdminAttributeRepository
{
    Task<ApiResponse<BackendPaginationResponse<List<BackendAttributeMasterGrid>>>> GetAttributeListAsync(AttributeMasterListRequest request, CancellationToken ct = default);
    Task<ApiResponse<BackendAttributeMasterDetail>> GetAttributeByIdAsync(AttributeMasterGetByIdRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> CreateAttributeAsync(CreateAttributeMasterRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> UpdateAttributeAsync(UpdateAttributeMasterRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> UpdateAttributeStatusAsync(AttributeMasterStatusUpdateRequest request, CancellationToken ct = default);
    Task<ApiResponse<BackendPaginationResponse<List<BackendAttributeValueGrid>>>> GetAttributeValueListAsync(AttributeValueListRequest request, CancellationToken ct = default);
    Task<ApiResponse<BackendAttributeValueDetail>> GetAttributeValueByIdAsync(AttributeValueGetByIdRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> CreateAttributeValueAsync(CreateAttributeValueRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> UpdateAttributeValueAsync(UpdateAttributeValueRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> UpdateAttributeValueStatusAsync(AttributeValueStatusUpdateRequest request, CancellationToken ct = default);
    Task<ApiResponse<List<VariantAttributeMappingDto>>> GetVariantAttributeMappingsAsync(VariantAttributeMappingGetByVariantIdRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> SaveVariantAttributeMappingsAsync(SaveVariantAttributeMappingRequest request, CancellationToken ct = default);
}

public interface IRolePermissionRepository
{
    Task<ApiResponse<BackendPaginationResponse<List<BackendRoleGrid>>>> GetRoleListAsync(RoleListRequest request, CancellationToken ct = default);
    Task<ApiResponse<BackendRoleDetail>> GetRoleByIdAsync(RoleGetByIdRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> CreateRoleAsync(CreateRoleRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> UpdateRoleAsync(UpdateRoleRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> UpdateRoleStatusAsync(RoleStatusUpdateRequest request, CancellationToken ct = default);
    Task<ApiResponse<List<PermissionOptionDto>>> GetPermissionOptionsAsync(CancellationToken ct = default);
}

public interface IAdminArchiveRepository
{
    Task<ApiResponse<RunArchiveDto>> RunArchiveAsync(RunArchiveRequest request, CancellationToken ct = default);
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
    Task<ApiResponse<AdminOperationResultDto>> ReleaseExpiredReservationsAsync(ReleaseExpiredCartReservationsRequest request, CancellationToken ct = default);
    Task<ApiResponse<AdminOperationResultDto>> RecalculateOrderTotalsAsync(RecalculateOrderTotalsRequest request, CancellationToken ct = default);
    Task<ApiResponse<AdminDataIntegrityCheckDto>> GetDataIntegrityCheckAsync(CancellationToken ct = default);
    Task<ApiResponse<BackendPaginationResponse<List<BackendInvoiceAdminGrid>>>> GetInvoiceListAsync(PaginationRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> GenerateInvoiceAsync(GenerateInvoiceRequest request, CancellationToken ct = default);
    Task<ApiResponse<BackendInvoiceAdminDetail>> GetInvoiceByIdAsync(InvoiceGetByIdRequest request, CancellationToken ct = default);
    Task<ApiResponse<BackendInvoicePrintDocument>> GetInvoicePrintDocumentAsync(InvoicePrintDocumentRequest request, CancellationToken ct = default);
    Task<ApiResponse<BackendPaginationResponse<List<BackendPaymentAdminGrid>>>> GetPaymentListAsync(PaginationRequest request, CancellationToken ct = default);
    Task<ApiResponse<object>> ProcessRefundAsync(ProcessPaymentRefundRequest request, CancellationToken ct = default);
}
