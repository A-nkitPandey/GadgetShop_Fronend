using GadgetShop.Application.Interfaces;
using GadgetShop.Core.Mapping;
using GadgetShop.Models;

namespace GadgetShop.Services;

public sealed class AdminDashboardService(IAdminDashboardRepository repository) : IAdminDashboardService
{
    public async Task<ApiResponse<AdminDashboardSummaryDto>> GetSummaryAsync(CancellationToken ct = default)
    {
        var response = await repository.GetSummaryAsync(ct);
        return response.MapData(data => new AdminDashboardSummaryDto
        {
            TotalProducts = data.TotalProducts,
            PendingOrders = data.PendingOrders,
            TotalOrders = data.TotalOrders,
            TotalRevenue = data.TotalSales,
            LowStockProducts = data.LowStockProducts
        });
    }

    public async Task<ApiResponse<AdminDashboardOverviewDto>> GetOverviewAsync(CancellationToken ct = default)
    {
        var response = await repository.GetOverviewAsync(ct);
        return response.MapData(data => new AdminDashboardOverviewDto
        {
            Summary = new AdminDashboardSummaryDto
            {
                TotalProducts = data.Summary.TotalProducts,
                PendingOrders = data.Summary.PendingOrders,
                TotalOrders = data.Summary.TotalOrders,
                TotalRevenue = data.Summary.TotalSales,
                LowStockProducts = data.Summary.LowStockProducts
            },
            TopProducts = data.TopSellingProducts.Select(product => new TopProductDto
            {
                ProductName = product.ProductName,
                SoldQty = product.TotalQuantitySold,
                Revenue = product.TotalSalesAmount
            }).ToList(),
            RecentOrders = data.RecentOrders.Select(order => new RecentOrderDto
            {
                OrderId = order.OrderId,
                OrderNo = order.OrderNo,
                CustomerName = order.CustomerName,
                Amount = order.TotalAmount,
                Status = order.OrderStatus,
                CreatedAt = order.PlacedAt
            }).ToList()
        });
    }

    public Task<ApiResponse<object>> GetReportAsync(AdminDashboardReportRequest request, CancellationToken ct = default) => repository.GetReportAsync(request, ct);
}

public sealed class AdminProductService(IAdminProductRepository repository) : IAdminProductService, IAdminCategoryService, IAdminBrandService, IAdminCouponService
{
    public async Task<ApiResponse<PagedResult<AdminProductDto>>> GetProductListAsync(PaginationRequest request, CancellationToken ct = default)
    {
        var response = await repository.GetProductListAsync(request, ct);
        return response.MapData(data => data.ToPagedResult(request.PageNo, request.PageSize, product => new AdminProductDto
        {
            Id = product.Id,
            ProductCode = product.ProductCode,
            ProductName = product.ProductName,
            CategoryName = product.CategoryName,
            BrandName = product.BrandName,
            BasePrice = product.BasePrice,
            StockQuantity = product.StockQuantity,
            IsActive = product.IsActive,
            PrimaryImageUrl = product.ImageUrl
        }));
    }

    public async Task<ApiResponse<AdminProductDto>> GetProductByIdAsync(ProductGetByIdRequest request, CancellationToken ct = default)
    {
        var response = await repository.GetProductByIdAsync(request, ct);
        return response.MapData(product => new AdminProductDto
        {
            Id = product.Id,
            ProductCode = product.ProductCode,
            ProductName = product.ProductName,
            CategoryId = product.CategoryId,
            CategoryName = product.CategoryName,
            BrandId = product.BrandId,
            BrandName = product.BrandName,
            BasePrice = product.BasePrice,
            Mrp = product.Mrp,
            CostPrice = null,
            CurrencyCode = product.CurrencyCode,
            TrackInventory = product.TrackInventory,
            StockQuantity = product.StockQuantity,
            ReservedQuantity = product.ReservedQuantity,
            ReorderLevel = product.ReorderLevel,
            IsActive = product.IsActive,
            HasVariants = product.HasVariants,
            Description = product.Description,
            PrimaryImageUrl = product.ImageUrl
        });
    }
    public Task<ApiResponse<object>> CreateProductAsync(CreateProductRequest request, CancellationToken ct = default) => repository.CreateProductAsync(request, ct);
    public Task<ApiResponse<object>> UpdateProductAsync(UpdateProductRequest request, CancellationToken ct = default) => repository.UpdateProductAsync(request, ct);
    public Task<ApiResponse<object>> UpdateProductStatusAsync(ProductStatusUpdateRequest request, CancellationToken ct = default) => repository.UpdateProductStatusAsync(request, ct);
    public Task<ApiResponse<ProductImageUploadDto>> UploadProductImageAsync(ProductImageUploadRequest request, CancellationToken ct = default) => repository.UploadProductImageAsync(request, ct);
    public Task<ApiResponse<ProductGalleryImageDto>> UploadProductGalleryImageAsync(ProductGalleryImageUploadRequest request, CancellationToken ct = default) => repository.UploadProductGalleryImageAsync(request, ct);
    public Task<ApiResponse<List<ProductGalleryImageDto>>> GetProductGalleryAsync(ProductGalleryGetByProductIdRequest request, CancellationToken ct = default) => repository.GetProductGalleryAsync(request, ct);
    public Task<ApiResponse<object>> DeleteProductGalleryImageAsync(ProductGalleryImageDeleteRequest request, CancellationToken ct = default) => repository.DeleteProductGalleryImageAsync(request, ct);
    public Task<ApiResponse<object>> SetPrimaryProductGalleryImageAsync(ProductPrimaryImageUpdateRequest request, CancellationToken ct = default) => repository.SetPrimaryProductGalleryImageAsync(request, ct);
    public Task<ApiResponse<GenerateProductContentResponseModel>> GenerateAIDescriptionAsync(GenerateProductContentRequestModel request, CancellationToken ct = default) => repository.GenerateAIDescriptionAsync(request, ct);

    public async Task<ApiResponse<PagedResult<CategoryDto>>> GetCategoryListAsync(PaginationRequest request, CancellationToken ct = default)
    {
        var response = await repository.GetCategoryListAsync(request, ct);
        return response.MapData(data => data.ToPagedResult(request.PageNo, request.PageSize, category => new CategoryDto
        {
            Id = category.Id,
            CategoryCode = category.CategoryCode,
            CategoryName = category.CategoryName,
            IsActive = category.IsActive,
            DisplayOrder = category.DisplayOrder,
            ParentCategoryName = category.ParentCategoryName
        }));
    }

    public Task<ApiResponse<object>> CreateCategoryAsync(CreateCategoryRequest request, CancellationToken ct = default) => repository.CreateCategoryAsync(request, ct);
    public Task<ApiResponse<object>> UpdateCategoryAsync(UpdateCategoryRequest request, CancellationToken ct = default) => repository.UpdateCategoryAsync(request, ct);
    public Task<ApiResponse<object>> UpdateCategoryStatusAsync(CategoryStatusUpdateRequest request, CancellationToken ct = default) => repository.UpdateCategoryStatusAsync(request, ct);

    public async Task<ApiResponse<PagedResult<BrandDto>>> GetBrandListAsync(PaginationRequest request, CancellationToken ct = default)
    {
        var response = await repository.GetBrandListAsync(request, ct);
        return response.MapData(data => data.ToPagedResult(request.PageNo, request.PageSize, brand => new BrandDto
        {
            Id = brand.Id,
            BrandCode = brand.BrandCode,
            BrandName = brand.BrandName,
            IsActive = brand.IsActive
        }));
    }

    public Task<ApiResponse<object>> CreateBrandAsync(CreateBrandRequest request, CancellationToken ct = default) => repository.CreateBrandAsync(request, ct);
    public Task<ApiResponse<object>> UpdateBrandAsync(UpdateBrandRequest request, CancellationToken ct = default) => repository.UpdateBrandAsync(request, ct);
    public Task<ApiResponse<object>> UpdateBrandStatusAsync(BrandStatusUpdateRequest request, CancellationToken ct = default) => repository.UpdateBrandStatusAsync(request, ct);

    public async Task<ApiResponse<PagedResult<CouponDto>>> GetCouponListAsync(PaginationRequest request, CancellationToken ct = default)
    {
        var response = await repository.GetCouponListAsync(request, ct);
        return response.MapData(data => data.ToPagedResult(request.PageNo, request.PageSize, coupon => new CouponDto
        {
            Id = coupon.Id,
            CouponCode = coupon.CouponCode,
            CouponName = coupon.CouponName,
            DiscountType = coupon.DiscountType,
            DiscountValue = coupon.DiscountValue,
            ValidFrom = coupon.ValidFrom,
            ValidTo = coupon.ValidTo,
            IsActive = coupon.IsActive
        }));
    }

    public Task<ApiResponse<object>> CreateCouponAsync(CreateCouponRequest request, CancellationToken ct = default) => repository.CreateCouponAsync(request, ct);
    public Task<ApiResponse<object>> UpdateCouponAsync(UpdateCouponRequest request, CancellationToken ct = default) => repository.UpdateCouponAsync(request, ct);
    public Task<ApiResponse<object>> UpdateCouponStatusAsync(CouponStatusUpdateRequest request, CancellationToken ct = default) => repository.UpdateCouponStatusAsync(request, ct);
}

public sealed class AdminUserService(IAdminUserRepository repository) : IAdminUserService
{
    public async Task<ApiResponse<PagedResult<AdminUserDto>>> GetUserListAsync(AdminUserListRequest request, CancellationToken ct = default)
    {
        var response = await repository.GetUserListAsync(request, ct);
        return response.MapData(data => data.ToPagedResult(request.PageNo, request.PageSize, user => new AdminUserDto
        {
            Id = user.Id,
            UserName = user.UserName,
            FullName = user.FullName,
            Email = user.Email,
            IsActive = user.IsActive,
            Roles = new List<string> { string.IsNullOrWhiteSpace(user.RoleName) ? user.RoleCode : user.RoleName }
        }));
    }

    public async Task<ApiResponse<AdminUserDto>> GetUserByIdAsync(AdminUserGetByIdRequest request, CancellationToken ct = default)
    {
        var response = await repository.GetUserByIdAsync(request, ct);
        return response.MapData(user => new AdminUserDto
        {
            Id = user.Id,
            UserName = user.UserName,
            FullName = user.FullName,
            Email = user.Email,
            IsActive = user.IsActive,
            Roles = new List<string> { string.IsNullOrWhiteSpace(user.RoleName) ? user.RoleCode : user.RoleName }
        });
    }

    public Task<ApiResponse<object>> UpdateUserAsync(UpdateAdminUserRequest request, CancellationToken ct = default)
    {
        request.RoleCode = ResolveRoleCode(request.RoleCode, request.Roles);
        return repository.UpdateUserAsync(request, ct);
    }
    public Task<ApiResponse<object>> UpdateUserStatusAsync(AdminUserStatusUpdateRequest request, CancellationToken ct = default) => repository.UpdateUserStatusAsync(request, ct);
    public Task<ApiResponse<object>> ResetPasswordAsync(AdminUserPasswordResetRequest request, CancellationToken ct = default) => repository.ResetPasswordAsync(request, ct);
    public Task<ApiResponse<List<LookupItem>>> GetRoleOptionsAsync(CancellationToken ct = default) => repository.GetRoleOptionsAsync(ct);

    public Task<ApiResponse<object>> CreateUserAsync(CreateAdminUserRequest request, CancellationToken ct = default)
    {
        request.RoleCode = ResolveRoleCode(request.RoleCode, request.Roles);
        request.UserName = string.IsNullOrWhiteSpace(request.UserName) ? request.Email : request.UserName;
        return repository.CreateUserAsync(request, ct);
    }

    private static string ResolveRoleCode(string existingRoleCode, List<string>? roles)
    {
        if (!string.IsNullOrWhiteSpace(existingRoleCode))
            return existingRoleCode;

        return roles?.FirstOrDefault() ?? string.Empty;
    }
}

public sealed class AdminInventoryService(IAdminInventoryRepository repository) : IAdminInventoryService
{
    public Task<ApiResponse<object>> StockInAsync(InventoryAdjustmentRequest request, CancellationToken ct = default) => repository.StockInAsync(request, ct);
    public Task<ApiResponse<object>> StockOutAsync(InventoryAdjustmentRequest request, CancellationToken ct = default) => repository.StockOutAsync(request, ct);

    public async Task<ApiResponse<PagedResult<InventoryTransactionDto>>> GetTransactionListAsync(ProductInventoryTransactionListRequest request, CancellationToken ct = default)
    {
        var response = await repository.GetTransactionListAsync(request, ct);
        return response.MapData(data => data.ToPagedResult(request.PageNo, request.PageSize, item => new InventoryTransactionDto
        {
            Id = item.Id,
            ProductCode = item.ProductCode,
            ProductName = item.ProductName,
            VariantName = item.VariantName,
            TransactionType = item.TransactionType,
            Quantity = item.Quantity,
            ReferenceNo = item.ReferenceNo,
            CreatedAt = item.CreatedAt,
            CreatedBy = item.CreatedBy
        }));
    }

    public async Task<ApiResponse<List<InventoryLowStockDto>>> GetInventoryReportAsync(InventoryAdminReportRequest request, CancellationToken ct = default)
    {
        var response = await repository.GetInventoryReportAsync(request, ct);
        return response.MapData(data => data.LowStockItems.Select(item => new InventoryLowStockDto
        {
            ProductId = item.ProductId,
            ProductCode = item.ProductCode,
            ProductName = item.ProductName,
            VariantName = item.VariantName,
            AvailableQuantity = item.AvailableQuantity,
            ReorderLevel = item.ReorderLevel
        }).ToList());
    }
}

public sealed class AdminCatalogAdminService(IAdminOperationsRepository repository) : IAdminAuditLogService, IAdminInvoiceService, IAdminPaymentService, IAdminOperationsService
{
    public async Task<ApiResponse<PagedResult<AuditLogDto>>> GetAuditLogListAsync(AuditLogListRequest request, CancellationToken ct = default)
    {
        var response = await repository.GetAuditLogListAsync(request, ct);
        return response.MapData(data => data.ToPagedResult(request.PageNo, request.PageSize, log => new AuditLogDto
        {
            Id = log.Id,
            ControllerName = log.ControllerName,
            ActionName = log.ActionName,
            HttpMethod = log.HttpMethod,
            UserCode = log.UserCode,
            ResponseCode = log.ResponseCode,
            IsSuccess = log.IsSuccess,
            CreatedAt = log.CreatedAt
        }));
    }

    public Task<ApiResponse<AdminOperationResultDto>> ReleaseExpiredReservationsAsync(ReleaseExpiredCartReservationsRequest request, CancellationToken ct = default) =>
        repository.ReleaseExpiredReservationsAsync(request, ct);

    public Task<ApiResponse<AdminOperationResultDto>> RecalculateOrderTotalsAsync(RecalculateOrderTotalsRequest request, CancellationToken ct = default) =>
        repository.RecalculateOrderTotalsAsync(request, ct);

    public Task<ApiResponse<AdminDataIntegrityCheckDto>> GetDataIntegrityCheckAsync(CancellationToken ct = default) => repository.GetDataIntegrityCheckAsync(ct);

    public async Task<ApiResponse<PagedResult<InvoiceAdminDto>>> GetInvoiceListAsync(PaginationRequest request, CancellationToken ct = default)
    {
        var response = await repository.GetInvoiceListAsync(request, ct);
        return response.MapData(data => data.ToPagedResult(request.PageNo, request.PageSize, invoice => new InvoiceAdminDto
        {
            Id = invoice.Id,
            OrderId = invoice.OrderId,
            OrderNo = invoice.OrderNo,
            InvoiceNo = invoice.InvoiceNo,
            CustomerName = invoice.CustomerName,
            GrandTotal = invoice.GrandTotal,
            GeneratedAt = invoice.GeneratedAt
        }));
    }

    public Task<ApiResponse<object>> GenerateInvoiceAsync(GenerateInvoiceRequest request, CancellationToken ct = default) => repository.GenerateInvoiceAsync(request, ct);

    public async Task<ApiResponse<InvoiceAdminDetailDto>> GetInvoiceByIdAsync(InvoiceGetByIdRequest request, CancellationToken ct = default)
    {
        var response = await repository.GetInvoiceByIdAsync(request, ct);
        return response.MapData(invoice => new InvoiceAdminDetailDto
        {
            Id = invoice.Id,
            OrderId = invoice.OrderId,
            OrderNo = invoice.OrderNo,
            InvoiceNo = invoice.InvoiceNo,
            CustomerName = invoice.CustomerName,
            TaxableAmount = invoice.TaxableAmount,
            TaxAmount = invoice.TaxAmount,
            DiscountAmount = invoice.DiscountAmount,
            GrandTotal = invoice.GrandTotal,
            GeneratedAt = invoice.GeneratedAt,
            GeneratedBy = invoice.GeneratedBy
        });
    }

    public async Task<ApiResponse<InvoicePrintDocumentDto>> GetInvoicePrintDocumentAsync(InvoicePrintDocumentRequest request, CancellationToken ct = default)
    {
        var response = await repository.GetInvoicePrintDocumentAsync(request, ct);
        return response.MapData(document => new InvoicePrintDocumentDto
        {
            Id = document.Id,
            InvoiceNo = document.InvoiceNo,
            OrderNo = document.OrderNo,
            CustomerName = document.CustomerName,
            CurrencyCode = document.CurrencyCode,
            TaxableAmount = document.TaxableAmount,
            TaxAmount = document.TaxAmount,
            DiscountAmount = document.DiscountAmount,
            GrandTotal = document.GrandTotal,
            GeneratedAt = document.GeneratedAt,
            SuggestedFileName = document.SuggestedFileName,
            PdfFileName = document.PdfFileName,
            PdfMimeType = document.PdfMimeType,
            PdfContentBase64 = document.PdfContentBase64,
            HtmlContent = document.HtmlContent
        });
    }

    public async Task<ApiResponse<PagedResult<PaymentAdminDto>>> GetPaymentListAsync(PaginationRequest request, CancellationToken ct = default)
    {
        var response = await repository.GetPaymentListAsync(request, ct);
        return response.MapData(data => data.ToPagedResult(request.PageNo, request.PageSize, payment => new PaymentAdminDto
        {
            Id = payment.Id,
            PaymentNo = payment.PaymentNo,
            OrderNo = payment.OrderNo,
            CustomerName = payment.CustomerName,
            Amount = payment.Amount,
            RefundedAmount = payment.RefundedAmount,
            CurrencyCode = payment.CurrencyCode,
            GatewayName = payment.GatewayName,
            TransactionId = payment.TransactionId,
            PaymentStatus = payment.PaymentStatus,
            PaymentDate = payment.PaymentDate,
            RefundedAt = payment.RefundedAt
        }));
    }

    public Task<ApiResponse<object>> ProcessRefundAsync(ProcessPaymentRefundRequest request, CancellationToken ct = default) => repository.ProcessRefundAsync(request, ct);
}

public sealed class AdminOrderService(IOrderRepository repository) : IAdminOrderService
{
    public async Task<ApiResponse<PagedResult<OrderDto>>> GetOrderListAsync(OrderListRequest request, CancellationToken ct = default)
    {
        var response = await repository.GetOrderListAsync(request, ct);
        return response.MapData(data => data.ToPagedResult(request.PageNo, request.PageSize, order => new OrderDto
        {
            Id = order.Id,
            OrderNo = order.OrderNo,
            CustomerName = order.FullName,
            Status = order.OrderStatus,
            PaymentStatus = order.PaymentStatus,
            TotalQuantity = order.TotalQuantity,
            TotalAmount = order.TotalAmount,
            CurrencyCode = order.CurrencyCode,
            CreatedAt = order.PlacedAt
        }));
    }

    public async Task<ApiResponse<OrderDto>> GetOrderByIdAsync(OrderGetByIdRequest request, CancellationToken ct = default)
    {
        var response = await repository.GetOrderByIdAsync(request, ct);
        return response.MapData(data => new OrderDto
        {
            Id = data.Id,
            OrderNo = data.OrderNo,
            CustomerName = data.FullName,
            Status = data.OrderStatus,
            PaymentStatus = data.PaymentStatus,
            TotalQuantity = data.Items.Sum(item => item.Quantity),
            TotalAmount = data.TotalAmount,
            CurrencyCode = data.CurrencyCode,
            CreatedAt = data.PlacedAt,
            Items = data.Items.Select(item => new OrderItemDto
            {
                Id = item.Id,
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                TotalPrice = item.LineTotal
            }).ToList()
        });
    }

    public Task<ApiResponse<object>> UpdateOrderStatusAsync(UpdateOrderStatusRequest request, CancellationToken ct = default) =>
        repository.UpdateOrderStatusAsync(request, ct);

    public Task<ApiResponse<object>> CreateShipmentAsync(CreateOrderShipmentRequest request, CancellationToken ct = default) =>
        repository.CreateShipmentAsync(request, ct);

    public Task<ApiResponse<object>> UpdateShipmentStatusAsync(UpdateOrderShipmentStatusRequest request, CancellationToken ct = default) =>
        repository.UpdateShipmentStatusAsync(request, ct);

    public async Task<ApiResponse<PagedResult<OrderShipmentDto>>> GetShipmentListAsync(OrderShipmentListRequest request, CancellationToken ct = default)
    {
        var response = await repository.GetShipmentListAsync(request, ct);
        return response.MapData(data => data.ToPagedResult(request.PageNo, request.PageSize, shipment => new OrderShipmentDto
        {
            Id = shipment.Id,
            OrderId = shipment.OrderId,
            OrderNo = shipment.OrderNo,
            ShipmentNo = shipment.ShipmentNo,
            ShipmentStatus = shipment.ShipmentStatus,
            CourierPartner = shipment.CourierPartner,
            TrackingNo = shipment.TrackingNo,
            ShippedAt = shipment.ShippedAt,
            DeliveredAt = shipment.DeliveredAt
        }));
    }

    public async Task<ApiResponse<OrderShipmentDto>> GetShipmentByOrderIdAsync(OrderShipmentGetByOrderIdRequest request, CancellationToken ct = default)
    {
        var response = await repository.GetShipmentByOrderIdAsync(request, ct);
        return response.MapData(shipment => new OrderShipmentDto
        {
            Id = shipment.Id,
            OrderId = shipment.OrderId,
            OrderNo = shipment.OrderNo,
            ShipmentNo = shipment.ShipmentNo,
            ShipmentStatus = shipment.ShipmentStatus,
            CourierPartner = shipment.CourierPartner,
            TrackingNo = shipment.TrackingNo,
            ShippedAt = shipment.ShippedAt,
            DeliveredAt = shipment.DeliveredAt,
            Remarks = shipment.Remarks
        });
    }

    public async Task<ApiResponse<PagedResult<OrderReturnDto>>> GetReturnListAsync(OrderReturnListRequest request, CancellationToken ct = default)
    {
        var response = await repository.GetReturnListAsync(request, ct);
        return response.MapData(data => data.ToPagedResult(request.PageNo, request.PageSize, item => new OrderReturnDto
        {
            Id = item.Id,
            OrderId = item.OrderId,
            OrderNo = item.OrderNo,
            ReturnNo = item.ReturnNo,
            ReturnStatus = item.ReturnStatus,
            RefundStatus = item.RefundStatus,
            RequestedAt = item.RequestedAt,
            CompletedAt = item.CompletedAt
        }));
    }

    public Task<ApiResponse<object>> UpdateReturnStatusAsync(UpdateOrderReturnStatusRequest request, CancellationToken ct = default) =>
        repository.UpdateReturnStatusAsync(request, ct);
}

public sealed class AdminVariantService(IAdminVariantRepository repository) : IAdminVariantService
{
    public async Task<ApiResponse<PagedResult<AdminProductVariantDto>>> GetVariantListAsync(ProductVariantListRequest request, CancellationToken ct = default)
    {
        var response = await repository.GetVariantListAsync(request, ct);
        return response.MapData(data => data.ToPagedResult(request.PageNo, request.PageSize, item => new AdminProductVariantDto
        {
            Id = item.Id,
            ProductId = item.ProductId,
            ProductCode = item.ProductCode,
            ProductName = item.ProductName,
            SkuCode = item.SkuCode,
            VariantName = item.VariantName,
            AttributeSummary = item.AttributeSummary,
            BasePrice = item.BasePrice,
            Mrp = item.Mrp,
            CurrencyCode = item.CurrencyCode,
            AvailableQuantity = item.AvailableQuantity,
            IsActive = item.IsActive
        }));
    }

    public async Task<ApiResponse<AdminProductVariantDto>> GetVariantByIdAsync(ProductVariantGetByIdRequest request, CancellationToken ct = default)
    {
        var response = await repository.GetVariantByIdAsync(request, ct);
        return response.MapData(item => new AdminProductVariantDto
        {
            Id = item.Id,
            ProductId = item.ProductId,
            ProductCode = item.ProductCode,
            ProductName = item.ProductName,
            SkuCode = item.SkuCode,
            VariantName = item.VariantName,
            AttributeSummary = item.AttributeSummary,
            BasePrice = item.BasePrice,
            Mrp = item.Mrp,
            CurrencyCode = item.CurrencyCode,
            TrackInventory = item.TrackInventory,
            StockQuantity = item.StockQuantity,
            ReservedQuantity = item.ReservedQuantity,
            AvailableQuantity = item.AvailableQuantity,
            ReorderLevel = item.ReorderLevel,
            IsActive = item.IsActive,
            AttributeMappings = item.AttributeMappings.Select(mapping => new VariantAttributeMappingDto
            {
                AttributeId = mapping.AttributeId,
                AttributeName = mapping.AttributeName,
                AttributeValueId = mapping.AttributeValueId,
                ValueCode = mapping.ValueCode,
                ValueText = mapping.ValueText
            }).ToList()
        });
    }

    public Task<ApiResponse<object>> CreateVariantAsync(CreateProductVariantRequest request, CancellationToken ct = default) => repository.CreateVariantAsync(request, ct);
    public Task<ApiResponse<object>> UpdateVariantAsync(UpdateProductVariantRequest request, CancellationToken ct = default) => repository.UpdateVariantAsync(request, ct);
    public Task<ApiResponse<object>> UpdateVariantStatusAsync(ProductVariantStatusUpdateRequest request, CancellationToken ct = default) => repository.UpdateVariantStatusAsync(request, ct);
}

public sealed class AdminAttributeService(IAdminAttributeRepository repository) : IAdminAttributeService
{
    public async Task<ApiResponse<PagedResult<AttributeMasterDto>>> GetAttributeListAsync(AttributeMasterListRequest request, CancellationToken ct = default)
    {
        var response = await repository.GetAttributeListAsync(request, ct);
        return response.MapData(data => data.ToPagedResult(request.PageNo, request.PageSize, item => new AttributeMasterDto
        {
            Id = item.Id,
            AttributeCode = item.AttributeCode,
            AttributeName = item.AttributeName,
            DataType = item.DataType,
            IsVariantAttribute = item.IsVariantAttribute,
            IsFilterable = item.IsFilterable,
            DisplayOrder = item.DisplayOrder,
            IsActive = item.IsActive
        }));
    }

    public async Task<ApiResponse<AttributeMasterDto>> GetAttributeByIdAsync(AttributeMasterGetByIdRequest request, CancellationToken ct = default)
    {
        var response = await repository.GetAttributeByIdAsync(request, ct);
        return response.MapData(item => new AttributeMasterDto
        {
            Id = item.Id,
            AttributeCode = item.AttributeCode,
            AttributeName = item.AttributeName,
            DataType = item.DataType,
            IsVariantAttribute = item.IsVariantAttribute,
            IsFilterable = item.IsFilterable,
            DisplayOrder = item.DisplayOrder,
            IsActive = item.IsActive
        });
    }

    public Task<ApiResponse<object>> CreateAttributeAsync(CreateAttributeMasterRequest request, CancellationToken ct = default) => repository.CreateAttributeAsync(request, ct);
    public Task<ApiResponse<object>> UpdateAttributeAsync(UpdateAttributeMasterRequest request, CancellationToken ct = default) => repository.UpdateAttributeAsync(request, ct);
    public Task<ApiResponse<object>> UpdateAttributeStatusAsync(AttributeMasterStatusUpdateRequest request, CancellationToken ct = default) => repository.UpdateAttributeStatusAsync(request, ct);

    public async Task<ApiResponse<PagedResult<AttributeValueDto>>> GetAttributeValueListAsync(AttributeValueListRequest request, CancellationToken ct = default)
    {
        var response = await repository.GetAttributeValueListAsync(request, ct);
        return response.MapData(data => data.ToPagedResult(request.PageNo, request.PageSize, item => new AttributeValueDto
        {
            Id = item.Id,
            AttributeId = item.AttributeId,
            AttributeName = item.AttributeName,
            ValueCode = item.ValueCode,
            ValueText = item.ValueText,
            DisplayOrder = item.DisplayOrder,
            IsActive = item.IsActive
        }));
    }

    public async Task<ApiResponse<AttributeValueDto>> GetAttributeValueByIdAsync(AttributeValueGetByIdRequest request, CancellationToken ct = default)
    {
        var response = await repository.GetAttributeValueByIdAsync(request, ct);
        return response.MapData(item => new AttributeValueDto
        {
            Id = item.Id,
            AttributeId = item.AttributeId,
            AttributeName = item.AttributeName,
            ValueCode = item.ValueCode,
            ValueText = item.ValueText,
            DisplayOrder = item.DisplayOrder,
            IsActive = item.IsActive
        });
    }

    public Task<ApiResponse<object>> CreateAttributeValueAsync(CreateAttributeValueRequest request, CancellationToken ct = default) => repository.CreateAttributeValueAsync(request, ct);
    public Task<ApiResponse<object>> UpdateAttributeValueAsync(UpdateAttributeValueRequest request, CancellationToken ct = default) => repository.UpdateAttributeValueAsync(request, ct);
    public Task<ApiResponse<object>> UpdateAttributeValueStatusAsync(AttributeValueStatusUpdateRequest request, CancellationToken ct = default) => repository.UpdateAttributeValueStatusAsync(request, ct);
    public Task<ApiResponse<List<VariantAttributeMappingDto>>> GetVariantAttributeMappingsAsync(VariantAttributeMappingGetByVariantIdRequest request, CancellationToken ct = default) => repository.GetVariantAttributeMappingsAsync(request, ct);
    public Task<ApiResponse<object>> SaveVariantAttributeMappingsAsync(SaveVariantAttributeMappingRequest request, CancellationToken ct = default) => repository.SaveVariantAttributeMappingsAsync(request, ct);
}

public sealed class RolePermissionService(IRolePermissionRepository repository) : IRolePermissionService
{
    public async Task<ApiResponse<PagedResult<RoleDto>>> GetRoleListAsync(RoleListRequest request, CancellationToken ct = default)
    {
        var response = await repository.GetRoleListAsync(request, ct);
        return response.MapData(data => data.ToPagedResult(request.PageNo, request.PageSize, item => new RoleDto
        {
            Id = item.Id,
            RoleCode = item.RoleCode,
            RoleName = item.RoleName,
            IsActive = item.IsActive,
            PermissionCount = item.PermissionCount
        }));
    }

    public async Task<ApiResponse<RoleDto>> GetRoleByIdAsync(RoleGetByIdRequest request, CancellationToken ct = default)
    {
        var response = await repository.GetRoleByIdAsync(request, ct);
        return response.MapData(item => new RoleDto
        {
            Id = item.Id,
            RoleCode = item.RoleCode,
            RoleName = item.RoleName,
            IsActive = item.IsActive,
            Permissions = item.Permissions
        });
    }

    public Task<ApiResponse<object>> CreateRoleAsync(CreateRoleRequest request, CancellationToken ct = default) => repository.CreateRoleAsync(request, ct);
    public Task<ApiResponse<object>> UpdateRoleAsync(UpdateRoleRequest request, CancellationToken ct = default) => repository.UpdateRoleAsync(request, ct);
    public Task<ApiResponse<object>> UpdateRoleStatusAsync(RoleStatusUpdateRequest request, CancellationToken ct = default) => repository.UpdateRoleStatusAsync(request, ct);
    public Task<ApiResponse<List<PermissionOptionDto>>> GetPermissionOptionsAsync(CancellationToken ct = default) => repository.GetPermissionOptionsAsync(ct);
}

public sealed class AdminArchiveService(IAdminArchiveRepository repository) : IAdminArchiveService
{
    public Task<ApiResponse<RunArchiveDto>> RunArchiveAsync(RunArchiveRequest request, CancellationToken ct = default) => repository.RunArchiveAsync(request, ct);
}
