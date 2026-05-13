using GadgetShop.ApiClients;
using GadgetShop.Application.Interfaces;
using GadgetShop.Constants;
using GadgetShop.Models;
using System.Net.Http.Headers;

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

    public Task<ApiResponse<BackendProductDetail>> GetProductByIdAsync(ProductGetByIdRequest request, CancellationToken ct = default) =>
        api.PostAsync<ProductGetByIdRequest, BackendProductDetail>(ApiEndpoints.ProductMaster.GetProductById, request, ct);

    public Task<ApiResponse<object>> CreateProductAsync(CreateProductRequest request, CancellationToken ct = default) =>
        api.PostAsync<CreateProductRequest, object>(ApiEndpoints.ProductMaster.CreateProduct, request, ct);

    public Task<ApiResponse<object>> UpdateProductAsync(UpdateProductRequest request, CancellationToken ct = default) =>
        api.PostAsync<UpdateProductRequest, object>(ApiEndpoints.ProductMaster.UpdateProduct, request, ct);

    public Task<ApiResponse<object>> UpdateProductStatusAsync(ProductStatusUpdateRequest request, CancellationToken ct = default) =>
        api.PostAsync<ProductStatusUpdateRequest, object>(ApiEndpoints.ProductMaster.UpdateProductStatus, request, ct);

    public Task<ApiResponse<ProductImageUploadDto>> UploadProductImageAsync(ProductImageUploadRequest request, CancellationToken ct = default) =>
        api.PostMultipartAsync<ProductImageUploadDto>(ApiEndpoints.ProductMaster.UploadProductImage, BuildPrimaryImageContent(request), ct);

    public Task<ApiResponse<ProductGalleryImageDto>> UploadProductGalleryImageAsync(ProductGalleryImageUploadRequest request, CancellationToken ct = default) =>
        api.PostMultipartAsync<ProductGalleryImageDto>(ApiEndpoints.ProductMaster.UploadProductGalleryImage, BuildGalleryImageContent(request), ct);

    public Task<ApiResponse<List<ProductGalleryImageDto>>> GetProductGalleryAsync(ProductGalleryGetByProductIdRequest request, CancellationToken ct = default) =>
        api.PostAsync<ProductGalleryGetByProductIdRequest, List<ProductGalleryImageDto>>(ApiEndpoints.ProductMaster.GetProductGallery, request, ct);

    public Task<ApiResponse<object>> DeleteProductGalleryImageAsync(ProductGalleryImageDeleteRequest request, CancellationToken ct = default) =>
        api.PostAsync<ProductGalleryImageDeleteRequest, object>(ApiEndpoints.ProductMaster.DeleteProductGalleryImage, request, ct);

    public Task<ApiResponse<object>> SetPrimaryProductGalleryImageAsync(ProductPrimaryImageUpdateRequest request, CancellationToken ct = default) =>
        api.PostAsync<ProductPrimaryImageUpdateRequest, object>(ApiEndpoints.ProductMaster.SetPrimaryProductGalleryImage, request, ct);

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

    public Task<ApiResponse<GenerateProductContentResponseModel>> GenerateAIDescriptionAsync(GenerateProductContentRequestModel request, CancellationToken ct = default) =>
        api.PostAsync<GenerateProductContentRequestModel, GenerateProductContentResponseModel>(ApiEndpoints.AiProductContent.GenerateAIDescription, request, ct);

    private static MultipartFormDataContent BuildPrimaryImageContent(ProductImageUploadRequest request)
    {
        var content = new MultipartFormDataContent
        {
            { new StringContent(request.ProductId.ToString()), nameof(ProductImageUploadRequest.ProductId) }
        };

        if (request.ImageContent is { Length: > 0 })
        {
            var fileContent = new ByteArrayContent(request.ImageContent);
            fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(request.ContentType);
            content.Add(fileContent, "Image", request.FileName);
        }

        return content;
    }

    private static MultipartFormDataContent BuildGalleryImageContent(ProductGalleryImageUploadRequest request)
    {
        var content = new MultipartFormDataContent
        {
            { new StringContent(request.ProductId.ToString()), nameof(ProductGalleryImageUploadRequest.ProductId) },
            { new StringContent(request.DisplayOrder.ToString()), nameof(ProductGalleryImageUploadRequest.DisplayOrder) },
            { new StringContent(request.IsPrimary.ToString()), nameof(ProductGalleryImageUploadRequest.IsPrimary) }
        };

        if (request.ImageContent is { Length: > 0 })
        {
            var fileContent = new ByteArrayContent(request.ImageContent);
            fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(request.ContentType);
            content.Add(fileContent, "Image", request.FileName);
        }

        return content;
    }
}

public sealed class AdminVariantRepository(IApiClient api) : IAdminVariantRepository
{
    public Task<ApiResponse<BackendPaginationResponse<List<BackendProductVariantGrid>>>> GetVariantListAsync(ProductVariantListRequest request, CancellationToken ct = default) =>
        api.PostAsync<ProductVariantListRequest, BackendPaginationResponse<List<BackendProductVariantGrid>>>(ApiEndpoints.ProductVariant.GetVariantList, request, ct);

    public Task<ApiResponse<BackendProductVariantDetail>> GetVariantByIdAsync(ProductVariantGetByIdRequest request, CancellationToken ct = default) =>
        api.PostAsync<ProductVariantGetByIdRequest, BackendProductVariantDetail>(ApiEndpoints.ProductVariant.GetVariantById, request, ct);

    public Task<ApiResponse<object>> CreateVariantAsync(CreateProductVariantRequest request, CancellationToken ct = default) =>
        api.PostAsync<CreateProductVariantRequest, object>(ApiEndpoints.ProductVariant.CreateVariant, request, ct);

    public Task<ApiResponse<object>> UpdateVariantAsync(UpdateProductVariantRequest request, CancellationToken ct = default) =>
        api.PostAsync<UpdateProductVariantRequest, object>(ApiEndpoints.ProductVariant.UpdateVariant, request, ct);

    public Task<ApiResponse<object>> UpdateVariantStatusAsync(ProductVariantStatusUpdateRequest request, CancellationToken ct = default) =>
        api.PostAsync<ProductVariantStatusUpdateRequest, object>(ApiEndpoints.ProductVariant.UpdateVariantStatus, request, ct);
}

public sealed class AdminAttributeRepository(IApiClient api) : IAdminAttributeRepository
{
    public Task<ApiResponse<BackendPaginationResponse<List<BackendAttributeMasterGrid>>>> GetAttributeListAsync(AttributeMasterListRequest request, CancellationToken ct = default) =>
        api.PostAsync<AttributeMasterListRequest, BackendPaginationResponse<List<BackendAttributeMasterGrid>>>(ApiEndpoints.ProductAttribute.GetAttributeList, request, ct);

    public Task<ApiResponse<BackendAttributeMasterDetail>> GetAttributeByIdAsync(AttributeMasterGetByIdRequest request, CancellationToken ct = default) =>
        api.PostAsync<AttributeMasterGetByIdRequest, BackendAttributeMasterDetail>(ApiEndpoints.ProductAttribute.GetAttributeById, request, ct);

    public Task<ApiResponse<object>> CreateAttributeAsync(CreateAttributeMasterRequest request, CancellationToken ct = default) =>
        api.PostAsync<CreateAttributeMasterRequest, object>(ApiEndpoints.ProductAttribute.CreateAttribute, request, ct);

    public Task<ApiResponse<object>> UpdateAttributeAsync(UpdateAttributeMasterRequest request, CancellationToken ct = default) =>
        api.PostAsync<UpdateAttributeMasterRequest, object>(ApiEndpoints.ProductAttribute.UpdateAttribute, request, ct);

    public Task<ApiResponse<object>> UpdateAttributeStatusAsync(AttributeMasterStatusUpdateRequest request, CancellationToken ct = default) =>
        api.PostAsync<AttributeMasterStatusUpdateRequest, object>(ApiEndpoints.ProductAttribute.UpdateAttributeStatus, request, ct);

    public Task<ApiResponse<BackendPaginationResponse<List<BackendAttributeValueGrid>>>> GetAttributeValueListAsync(AttributeValueListRequest request, CancellationToken ct = default) =>
        api.PostAsync<AttributeValueListRequest, BackendPaginationResponse<List<BackendAttributeValueGrid>>>(ApiEndpoints.ProductAttribute.GetAttributeValueList, request, ct);

    public Task<ApiResponse<BackendAttributeValueDetail>> GetAttributeValueByIdAsync(AttributeValueGetByIdRequest request, CancellationToken ct = default) =>
        api.PostAsync<AttributeValueGetByIdRequest, BackendAttributeValueDetail>(ApiEndpoints.ProductAttribute.GetAttributeValueById, request, ct);

    public Task<ApiResponse<object>> CreateAttributeValueAsync(CreateAttributeValueRequest request, CancellationToken ct = default) =>
        api.PostAsync<CreateAttributeValueRequest, object>(ApiEndpoints.ProductAttribute.CreateAttributeValue, request, ct);

    public Task<ApiResponse<object>> UpdateAttributeValueAsync(UpdateAttributeValueRequest request, CancellationToken ct = default) =>
        api.PostAsync<UpdateAttributeValueRequest, object>(ApiEndpoints.ProductAttribute.UpdateAttributeValue, request, ct);

    public Task<ApiResponse<object>> UpdateAttributeValueStatusAsync(AttributeValueStatusUpdateRequest request, CancellationToken ct = default) =>
        api.PostAsync<AttributeValueStatusUpdateRequest, object>(ApiEndpoints.ProductAttribute.UpdateAttributeValueStatus, request, ct);

    public Task<ApiResponse<List<VariantAttributeMappingDto>>> GetVariantAttributeMappingsAsync(VariantAttributeMappingGetByVariantIdRequest request, CancellationToken ct = default) =>
        api.PostAsync<VariantAttributeMappingGetByVariantIdRequest, List<VariantAttributeMappingDto>>(ApiEndpoints.ProductAttribute.GetVariantAttributeMappings, request, ct);

    public Task<ApiResponse<object>> SaveVariantAttributeMappingsAsync(SaveVariantAttributeMappingRequest request, CancellationToken ct = default) =>
        api.PostAsync<SaveVariantAttributeMappingRequest, object>(ApiEndpoints.ProductAttribute.SaveVariantAttributeMappings, request, ct);
}

public sealed class RolePermissionRepository(IApiClient api) : IRolePermissionRepository
{
    public Task<ApiResponse<BackendPaginationResponse<List<BackendRoleGrid>>>> GetRoleListAsync(RoleListRequest request, CancellationToken ct = default) =>
        api.PostAsync<RoleListRequest, BackendPaginationResponse<List<BackendRoleGrid>>>(ApiEndpoints.RolePermission.GetRoleList, request, ct);

    public Task<ApiResponse<BackendRoleDetail>> GetRoleByIdAsync(RoleGetByIdRequest request, CancellationToken ct = default) =>
        api.PostAsync<RoleGetByIdRequest, BackendRoleDetail>(ApiEndpoints.RolePermission.GetRoleById, request, ct);

    public Task<ApiResponse<object>> CreateRoleAsync(CreateRoleRequest request, CancellationToken ct = default) =>
        api.PostAsync<CreateRoleRequest, object>(ApiEndpoints.RolePermission.CreateRole, request, ct);

    public Task<ApiResponse<object>> UpdateRoleAsync(UpdateRoleRequest request, CancellationToken ct = default) =>
        api.PostAsync<UpdateRoleRequest, object>(ApiEndpoints.RolePermission.UpdateRole, request, ct);

    public Task<ApiResponse<object>> UpdateRoleStatusAsync(RoleStatusUpdateRequest request, CancellationToken ct = default) =>
        api.PostAsync<RoleStatusUpdateRequest, object>(ApiEndpoints.RolePermission.UpdateRoleStatus, request, ct);

    public Task<ApiResponse<List<PermissionOptionDto>>> GetPermissionOptionsAsync(CancellationToken ct = default) =>
        api.GetAsync<List<PermissionOptionDto>>(ApiEndpoints.RolePermission.GetPermissionOptions, ct);
}

public sealed class AdminArchiveRepository(IApiClient api) : IAdminArchiveRepository
{
    public Task<ApiResponse<RunArchiveDto>> RunArchiveAsync(RunArchiveRequest request, CancellationToken ct = default) =>
        api.PostAsync<RunArchiveRequest, RunArchiveDto>(ApiEndpoints.AdminArchive.RunArchive, request, ct);
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

    public Task<ApiResponse<AdminOperationResultDto>> ReleaseExpiredReservationsAsync(ReleaseExpiredCartReservationsRequest request, CancellationToken ct = default) =>
        api.PostAsync<ReleaseExpiredCartReservationsRequest, AdminOperationResultDto>(ApiEndpoints.AdminOperations.ReleaseExpiredReservations, request, ct);

    public Task<ApiResponse<AdminOperationResultDto>> RecalculateOrderTotalsAsync(RecalculateOrderTotalsRequest request, CancellationToken ct = default) =>
        api.PostAsync<RecalculateOrderTotalsRequest, AdminOperationResultDto>(ApiEndpoints.AdminOperations.RecalculateOrderTotals, request, ct);

    public Task<ApiResponse<AdminDataIntegrityCheckDto>> GetDataIntegrityCheckAsync(CancellationToken ct = default) =>
        api.GetAsync<AdminDataIntegrityCheckDto>(ApiEndpoints.AdminOperations.GetDataIntegrityCheck, ct);

    public Task<ApiResponse<BackendPaginationResponse<List<BackendInvoiceAdminGrid>>>> GetInvoiceListAsync(PaginationRequest request, CancellationToken ct = default) =>
        api.PostAsync<PaginationRequest, BackendPaginationResponse<List<BackendInvoiceAdminGrid>>>(ApiEndpoints.Invoice.GetInvoiceList, request, ct);

    public Task<ApiResponse<object>> GenerateInvoiceAsync(GenerateInvoiceRequest request, CancellationToken ct = default) =>
        api.PostAsync<GenerateInvoiceRequest, object>(ApiEndpoints.Invoice.GenerateInvoice, request, ct);

    public Task<ApiResponse<BackendInvoiceAdminDetail>> GetInvoiceByIdAsync(InvoiceGetByIdRequest request, CancellationToken ct = default) =>
        api.PostAsync<InvoiceGetByIdRequest, BackendInvoiceAdminDetail>(ApiEndpoints.Invoice.GetInvoiceById, request, ct);

    public Task<ApiResponse<BackendInvoicePrintDocument>> GetInvoicePrintDocumentAsync(InvoicePrintDocumentRequest request, CancellationToken ct = default) =>
        api.PostAsync<InvoicePrintDocumentRequest, BackendInvoicePrintDocument>(ApiEndpoints.Invoice.GetInvoicePrintDocument, request, ct);

    public Task<ApiResponse<BackendPaginationResponse<List<BackendPaymentAdminGrid>>>> GetPaymentListAsync(PaginationRequest request, CancellationToken ct = default) =>
        api.PostAsync<PaginationRequest, BackendPaginationResponse<List<BackendPaymentAdminGrid>>>(ApiEndpoints.PaymentAdmin.GetPaymentList, request, ct);

    public Task<ApiResponse<object>> ProcessRefundAsync(ProcessPaymentRefundRequest request, CancellationToken ct = default) =>
        api.PostAsync<ProcessPaymentRefundRequest, object>(ApiEndpoints.PaymentAdmin.ProcessRefund, request, ct);
}
