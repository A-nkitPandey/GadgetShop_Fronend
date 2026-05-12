using GadgetShop.ApiClients;
using GadgetShop.Application.Interfaces;
using GadgetShop.Constants;
using GadgetShop.Models;

namespace GadgetShop.Infrastructure.Api.Repositories;

public sealed class AdminDashboardRepository(IApiClient api) : IAdminDashboardRepository
{
    public Task<ApiResponse<BackendAdminDashboardSummary>> GetSummaryAsync(CancellationToken ct = default) =>
        api.GetAsync<BackendAdminDashboardSummary>(ApiEndpoints.AdminDashboard.GetSummary, ct);

    public Task<ApiResponse<BackendAdminDashboardOverview>> GetOverviewAsync(CancellationToken ct = default) =>
        api.GetAsync<BackendAdminDashboardOverview>(ApiEndpoints.AdminDashboard.GetDashboardOverview, ct);

    public Task<ApiResponse<object>> GetReportAsync(AdminDashboardReportRequest request, CancellationToken ct = default) =>
        api.PostAsync<AdminDashboardReportRequest, object>(ApiEndpoints.AdminDashboard.GetDashboardReport, request, ct);
}

public sealed class AdminProductRepository(IApiClient api) : IAdminProductRepository
{
    public Task<ApiResponse<BackendPaginationResponse<List<BackendProductGrid>>>> GetProductListAsync(PaginationRequest request, CancellationToken ct = default) =>
        api.PostAsync<PaginationRequest, BackendPaginationResponse<List<BackendProductGrid>>>(ApiEndpoints.ProductMaster.GetProductList, request, ct);

    public Task<ApiResponse<AdminProductDto>> GetProductByIdAsync(ProductGetByIdRequest request, CancellationToken ct = default) =>
        api.PostAsync<ProductGetByIdRequest, AdminProductDto>(ApiEndpoints.ProductMaster.GetProductById, request, ct);

    public Task<ApiResponse<object>> CreateProductAsync(CreateProductRequest request, CancellationToken ct = default) =>
        api.PostAsync<CreateProductRequest, object>(ApiEndpoints.ProductMaster.CreateProduct, request, ct);

    public Task<ApiResponse<object>> UpdateProductAsync(UpdateProductRequest request, CancellationToken ct = default) =>
        api.PostAsync<UpdateProductRequest, object>(ApiEndpoints.ProductMaster.UpdateProduct, request, ct);

    public Task<ApiResponse<object>> UpdateProductStatusAsync(ProductStatusUpdateRequest request, CancellationToken ct = default) =>
        api.PostAsync<ProductStatusUpdateRequest, object>(ApiEndpoints.ProductMaster.UpdateProductStatus, request, ct);

    public Task<ApiResponse<BackendPaginationResponse<List<BackendCategoryGrid>>>> GetCategoryListAsync(PaginationRequest request, CancellationToken ct = default) =>
        api.PostAsync<PaginationRequest, BackendPaginationResponse<List<BackendCategoryGrid>>>(ApiEndpoints.CategoryMaster.GetCategoryList, request, ct);

    public Task<ApiResponse<object>> CreateCategoryAsync(CreateCategoryRequest request, CancellationToken ct = default) =>
        api.PostAsync<CreateCategoryRequest, object>(ApiEndpoints.CategoryMaster.CreateCategory, request, ct);

    public Task<ApiResponse<object>> UpdateCategoryAsync(UpdateCategoryRequest request, CancellationToken ct = default) =>
        api.PostAsync<UpdateCategoryRequest, object>(ApiEndpoints.CategoryMaster.UpdateCategory, request, ct);

    public Task<ApiResponse<object>> UpdateCategoryStatusAsync(CategoryStatusUpdateRequest request, CancellationToken ct = default) =>
        api.PostAsync<CategoryStatusUpdateRequest, object>(ApiEndpoints.CategoryMaster.UpdateCategoryStatus, request, ct);

    public Task<ApiResponse<BackendPaginationResponse<List<BackendBrandGrid>>>> GetBrandListAsync(PaginationRequest request, CancellationToken ct = default) =>
        api.PostAsync<PaginationRequest, BackendPaginationResponse<List<BackendBrandGrid>>>(ApiEndpoints.BrandMaster.GetBrandList, request, ct);

    public Task<ApiResponse<object>> CreateBrandAsync(CreateBrandRequest request, CancellationToken ct = default) =>
        api.PostAsync<CreateBrandRequest, object>(ApiEndpoints.BrandMaster.CreateBrand, request, ct);

    public Task<ApiResponse<object>> UpdateBrandAsync(UpdateBrandRequest request, CancellationToken ct = default) =>
        api.PostAsync<UpdateBrandRequest, object>(ApiEndpoints.BrandMaster.UpdateBrand, request, ct);

    public Task<ApiResponse<object>> UpdateBrandStatusAsync(BrandStatusUpdateRequest request, CancellationToken ct = default) =>
        api.PostAsync<BrandStatusUpdateRequest, object>(ApiEndpoints.BrandMaster.UpdateBrandStatus, request, ct);

    public Task<ApiResponse<BackendPaginationResponse<List<BackendCouponGrid>>>> GetCouponListAsync(PaginationRequest request, CancellationToken ct = default) =>
        api.PostAsync<PaginationRequest, BackendPaginationResponse<List<BackendCouponGrid>>>(ApiEndpoints.CouponMaster.GetCouponList, request, ct);

    public Task<ApiResponse<object>> CreateCouponAsync(CreateCouponRequest request, CancellationToken ct = default) =>
        api.PostAsync<CreateCouponRequest, object>(ApiEndpoints.CouponMaster.CreateCoupon, request, ct);

    public Task<ApiResponse<object>> UpdateCouponAsync(UpdateCouponRequest request, CancellationToken ct = default) =>
        api.PostAsync<UpdateCouponRequest, object>(ApiEndpoints.CouponMaster.UpdateCoupon, request, ct);

    public Task<ApiResponse<object>> UpdateCouponStatusAsync(CouponStatusUpdateRequest request, CancellationToken ct = default) =>
        api.PostAsync<CouponStatusUpdateRequest, object>(ApiEndpoints.CouponMaster.UpdateCouponStatus, request, ct);
}

public sealed class AdminUserRepository(IApiClient api) : IAdminUserRepository
{
    public Task<ApiResponse<BackendPaginationResponse<List<BackendAdminUserGrid>>>> GetUserListAsync(AdminUserListRequest request, CancellationToken ct = default) =>
        api.PostAsync<AdminUserListRequest, BackendPaginationResponse<List<BackendAdminUserGrid>>>(ApiEndpoints.AdminUser.GetUserList, request, ct);

    public Task<ApiResponse<BackendAdminUserGrid>> GetUserByIdAsync(AdminUserGetByIdRequest request, CancellationToken ct = default) =>
        api.PostAsync<AdminUserGetByIdRequest, BackendAdminUserGrid>(ApiEndpoints.AdminUser.GetUserById, request, ct);

    public Task<ApiResponse<object>> CreateUserAsync(CreateAdminUserRequest request, CancellationToken ct = default) =>
        api.PostAsync<CreateAdminUserRequest, object>(ApiEndpoints.AdminUser.CreateUser, request, ct);

    public Task<ApiResponse<object>> UpdateUserAsync(UpdateAdminUserRequest request, CancellationToken ct = default) =>
        api.PostAsync<UpdateAdminUserRequest, object>(ApiEndpoints.AdminUser.UpdateUser, request, ct);

    public Task<ApiResponse<object>> UpdateUserStatusAsync(AdminUserStatusUpdateRequest request, CancellationToken ct = default) =>
        api.PostAsync<AdminUserStatusUpdateRequest, object>(ApiEndpoints.AdminUser.UpdateUserStatus, request, ct);

    public Task<ApiResponse<object>> ResetPasswordAsync(AdminUserPasswordResetRequest request, CancellationToken ct = default) =>
        api.PostAsync<AdminUserPasswordResetRequest, object>(ApiEndpoints.AdminUser.ResetPassword, request, ct);

    public Task<ApiResponse<List<LookupItem>>> GetRoleOptionsAsync(CancellationToken ct = default) =>
        api.GetAsync<List<LookupItem>>(ApiEndpoints.AdminUser.GetRoleOptions, ct);
}

public sealed class AdminInventoryRepository(IApiClient api) : IAdminInventoryRepository
{
    public Task<ApiResponse<object>> StockInAsync(InventoryAdjustmentRequest request, CancellationToken ct = default) =>
        api.PostAsync<InventoryAdjustmentRequest, object>(ApiEndpoints.ProductInventory.StockIn, request, ct);

    public Task<ApiResponse<object>> StockOutAsync(InventoryAdjustmentRequest request, CancellationToken ct = default) =>
        api.PostAsync<InventoryAdjustmentRequest, object>(ApiEndpoints.ProductInventory.StockOut, request, ct);

    public Task<ApiResponse<BackendPaginationResponse<List<BackendInventoryTransaction>>>> GetTransactionListAsync(ProductInventoryTransactionListRequest request, CancellationToken ct = default) =>
        api.PostAsync<ProductInventoryTransactionListRequest, BackendPaginationResponse<List<BackendInventoryTransaction>>>(ApiEndpoints.ProductInventory.GetTransactionList, request, ct);

    public Task<ApiResponse<BackendInventoryReport>> GetInventoryReportAsync(InventoryAdminReportRequest request, CancellationToken ct = default) =>
        api.PostAsync<InventoryAdminReportRequest, BackendInventoryReport>(ApiEndpoints.ProductInventory.GetInventoryAdminReport, request, ct);
}

public sealed class AdminOperationsRepository(IApiClient api) : IAdminOperationsRepository
{
    public Task<ApiResponse<BackendPaginationResponse<List<BackendAuditLogGrid>>>> GetAuditLogListAsync(AuditLogListRequest request, CancellationToken ct = default) =>
        api.PostAsync<AuditLogListRequest, BackendPaginationResponse<List<BackendAuditLogGrid>>>(ApiEndpoints.AuditLog.GetAuditLogList, request, ct);

    public Task<ApiResponse<object>> GetDataIntegrityCheckAsync(CancellationToken ct = default) =>
        api.GetAsync<object>(ApiEndpoints.AdminOperations.GetDataIntegrityCheck, ct);

    public Task<ApiResponse<BackendPaginationResponse<List<BackendInvoiceAdminGrid>>>> GetInvoiceListAsync(PaginationRequest request, CancellationToken ct = default) =>
        api.PostAsync<PaginationRequest, BackendPaginationResponse<List<BackendInvoiceAdminGrid>>>(ApiEndpoints.Invoice.GetInvoiceList, request, ct);

    public Task<ApiResponse<object>> GenerateInvoiceAsync(GenerateInvoiceRequest request, CancellationToken ct = default) =>
        api.PostAsync<GenerateInvoiceRequest, object>(ApiEndpoints.Invoice.GenerateInvoice, request, ct);

    public Task<ApiResponse<BackendPaginationResponse<List<BackendPaymentAdminGrid>>>> GetPaymentListAsync(PaginationRequest request, CancellationToken ct = default) =>
        api.PostAsync<PaginationRequest, BackendPaginationResponse<List<BackendPaymentAdminGrid>>>(ApiEndpoints.PaymentAdmin.GetPaymentList, request, ct);

    public Task<ApiResponse<object>> ProcessRefundAsync(ProcessPaymentRefundRequest request, CancellationToken ct = default) =>
        api.PostAsync<ProcessPaymentRefundRequest, object>(ApiEndpoints.PaymentAdmin.ProcessRefund, request, ct);
}
