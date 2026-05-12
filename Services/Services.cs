using GadgetShop.ApiClients;
using GadgetShop.Authentication;
using GadgetShop.Constants;
using GadgetShop.Models;

namespace GadgetShop.Services;

public sealed class AuthService(IApiClient api, TokenStorageService tokenStorage, AuthStateProvider authState)
{
    public async Task<ApiResponse<LoginResponse>> LoginAsync(LoginRequest request, CancellationToken ct = default)
    {
        var response = await api.PostAsync<LoginRequest, LoginResponse>(ApiEndpoints.Auth.Login, request, ct);
        if (response.IsSuccess && response.Data?.AccessToken is { Length: > 0 } token)
        {
            await tokenStorage.SetAccessTokenAsync(token);
            await tokenStorage.SetUserAsync(response.Data);
            authState.NotifyAuthChanged();
        }

        return response;
    }

    public Task<ApiResponse<object>> RegisterAsync(CustomerRegisterRequest request, CancellationToken ct = default)
    {
        request.UserName = string.IsNullOrWhiteSpace(request.UserName) ? request.Email : request.UserName;
        return api.PostAsync<CustomerRegisterRequest, object>(ApiEndpoints.CustomerAccount.Register, request, ct);
    }

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

public sealed class CatalogService(IApiClient api)
{
    public async Task<ApiResponse<PagedResult<CustomerCatalogProductDto>>> GetProductsAsync(
        CustomerCatalogListRequest request, CancellationToken ct = default)
    {
        var response = await api.PostAsync<CustomerCatalogListRequest, BackendPaginationResponse<List<BackendCustomerCatalogListItem>>>(
            ApiEndpoints.CustomerCatalog.GetProductList, request, ct);

        return response.MapData(data => data.ToPagedResult(request.PageNo, request.PageSize, MapCatalogProduct));
    }

    public async Task<ApiResponse<CustomerCatalogProductDto>> GetProductByIdAsync(
        CustomerCatalogGetByIdRequest request, CancellationToken ct = default)
    {
        var response = await api.PostAsync<CustomerCatalogGetByIdRequest, BackendProductDetail>(
            ApiEndpoints.CustomerCatalog.GetProductById, request, ct);

        return response.MapData(MapProductDetail);
    }

    public async Task<ApiResponse<List<CustomerCatalogProductDto>>> GetRecommendationsAsync(
        long? productId = null, int pageSize = 8, CancellationToken ct = default)
    {
        var response = await api.PostAsync<BackendRecommendationRequest, BackendRecommendationResponse>(
            ApiEndpoints.CustomerCatalog.GetRecommendations,
            new BackendRecommendationRequest { ProductId = productId, PageSize = pageSize },
            ct);

        return response.MapData(data =>
            data.Sections.SelectMany(section => section.Products).Select(MapCatalogProduct).ToList());
    }

    public async Task<ApiResponse<PagedResult<ProductReviewDto>>> GetReviewsAsync(
        ProductReviewListRequest request, CancellationToken ct = default)
    {
        var response = await api.PostAsync<ProductReviewListRequest, BackendPaginationResponse<List<BackendProductReview>>>(
            ApiEndpoints.ProductReview.GetByProduct, request, ct);

        return response.MapData(data => data.ToPagedResult(request.PageNo, request.PageSize, MapReview));
    }

    public Task<ApiResponse<object>> SaveReviewAsync(ProductReviewMutationRequest request, CancellationToken ct = default)
        => api.PostAsync<ProductReviewMutationRequest, object>(ApiEndpoints.ProductReview.Save, request, ct);

    public Task<ApiResponse<object>> DeleteReviewAsync(ProductReviewDeleteRequest request, CancellationToken ct = default)
        => api.PostAsync<ProductReviewDeleteRequest, object>(ApiEndpoints.ProductReview.Delete, request, ct);

    private static CustomerCatalogProductDto MapCatalogProduct(BackendCustomerCatalogListItem item) =>
        new()
        {
            Id = item.Id,
            ProductCode = item.ProductCode,
            ProductName = item.ProductName,
            Description = item.Description,
            BasePrice = item.BasePrice,
            Mrp = item.Mrp,
            CurrencyCode = item.CurrencyCode,
            StockQuantity = item.InStock ? 1 : 0,
            IsInStock = item.InStock,
            PrimaryImageUrl = item.ImageUrl,
            CategoryName = item.CategoryName,
            BrandName = item.BrandName
        };

    private static CustomerCatalogProductDto MapProductDetail(BackendProductDetail item) =>
        new()
        {
            Id = item.Id,
            ProductCode = item.ProductCode,
            ProductName = item.ProductName,
            Description = item.Description,
            BasePrice = item.BasePrice,
            Mrp = item.Mrp,
            CurrencyCode = item.CurrencyCode,
            StockQuantity = item.AvailableQuantity > 0 ? item.AvailableQuantity : item.StockQuantity,
            IsInStock = (item.AvailableQuantity > 0 ? item.AvailableQuantity : item.StockQuantity) > 0,
            PrimaryImageUrl = item.ImageUrl ?? item.GalleryImages.OrderByDescending(image => image.IsPrimary).ThenBy(image => image.DisplayOrder).Select(image => image.ImageUrl).FirstOrDefault(),
            CategoryName = item.CategoryName,
            BrandName = item.BrandName,
            Variants = item.Variants.Select(variant => new ProductVariantDto
            {
                Id = variant.Id,
                SkuCode = variant.SkuCode,
                VariantName = variant.VariantName,
                Price = variant.BasePrice,
                StockQuantity = variant.AvailableQuantity > 0 ? variant.AvailableQuantity : variant.StockQuantity,
                Attributes = string.IsNullOrWhiteSpace(variant.AttributeSummary)
                    ? new()
                    : variant.AttributeSummary.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                        .Select(value => new VariantAttributeDto { ValueText = value })
                        .ToList()
            }).ToList(),
            GalleryImages = item.GalleryImages.OrderBy(image => image.DisplayOrder).Select(image => image.ImageUrl).ToList()
        };

    private static ProductReviewDto MapReview(BackendProductReview review) =>
        new()
        {
            Id = review.Id,
            ReviewerName = string.IsNullOrWhiteSpace(review.FullName) ? review.UserName : review.FullName,
            Rating = review.Rating,
            ReviewText = review.ReviewText,
            CreatedAt = review.CreatedAt
        };
}

public sealed class CartService(IApiClient api)
{
    public async Task<ApiResponse<CartDto>> GetMyCartAsync(CancellationToken ct = default)
    {
        var response = await api.GetAsync<BackendCart>(ApiEndpoints.Cart.GetMyCart, ct);
        return response.MapData(MapCart);
    }

    public Task<ApiResponse<CartDto>> AddItemAsync(AddCartItemRequest request, CancellationToken ct = default)
        => ExecuteAndReloadCartAsync(() => api.PostAsync<AddCartItemRequest, BackendCartMutation>(ApiEndpoints.Cart.AddItem, request, ct), ct);

    public Task<ApiResponse<CartDto>> UpdateQuantityAsync(UpdateCartItemQuantityRequest request, CancellationToken ct = default)
        => ExecuteAndReloadCartAsync(() => api.PostAsync<UpdateCartItemQuantityRequest, BackendCartMutation>(ApiEndpoints.Cart.UpdateItemQuantity, request, ct), ct);

    public Task<ApiResponse<CartDto>> RemoveItemAsync(RemoveCartItemRequest request, CancellationToken ct = default)
        => ExecuteAndReloadCartAsync(() => api.PostAsync<RemoveCartItemRequest, BackendCartMutation>(ApiEndpoints.Cart.RemoveItem, request, ct), ct);

    public Task<ApiResponse<CartDto>> ClearCartAsync(CancellationToken ct = default)
        => ExecuteAndReloadCartAsync(() => api.PostEmptyAsync<object>(ApiEndpoints.Cart.ClearCart, ct), ct);

    public Task<ApiResponse<CartDto>> ApplyCouponAsync(ApplyCartCouponRequest request, CancellationToken ct = default)
        => ExecuteAndReloadCartAsync(() => api.PostAsync<ApplyCartCouponRequest, BackendCart>(ApiEndpoints.Cart.ApplyCoupon, request, ct), ct);

    public Task<ApiResponse<CartDto>> RemoveCouponAsync(CancellationToken ct = default)
        => ExecuteAndReloadCartAsync(() => api.PostEmptyAsync<BackendCart>(ApiEndpoints.Cart.RemoveCoupon, ct), ct);

    private async Task<ApiResponse<CartDto>> ExecuteAndReloadCartAsync<TResponse>(Func<Task<ApiResponse<TResponse>>> action, CancellationToken ct)
    {
        var mutation = await action();
        if (!mutation.IsSuccess)
        {
            return new ApiResponse<CartDto>
            {
                Code = mutation.Code,
                Message = mutation.Message,
                Errors = mutation.Errors
            };
        }

        return await GetMyCartAsync(ct);
    }

    private static CartDto MapCart(BackendCart cart) =>
        new()
        {
            Id = cart.CartId,
            SubTotal = cart.SubTotal,
            DiscountAmount = cart.DiscountAmount,
            TotalAmount = cart.TotalAmount,
            AppliedCouponCode = cart.CouponCode,
            CurrencyCode = cart.CurrencyCode,
            Items = cart.Items.Select(item => new CartItemDto
            {
                Id = item.Id,
                ProductId = item.ProductId,
                VariantId = item.VariantId,
                ProductName = item.ProductName,
                VariantName = item.VariantName,
                UnitPrice = item.UnitPrice,
                Quantity = item.Quantity,
                TotalPrice = item.LineTotal,
                AvailableStock = item.AvailableQuantity
            }).ToList()
        };
}

public sealed class OrderService(IApiClient api)
{
    public async Task<ApiResponse<OrderDto>> PlaceOrderAsync(PlaceOrderRequest request, CancellationToken ct = default)
    {
        var response = await api.PostAsync<PlaceOrderRequest, BackendPlaceOrderResponse>(ApiEndpoints.Order.PlaceOrder, request, ct);
        return response.MapData(data => new OrderDto
        {
            Id = data.OrderId,
            OrderNo = data.OrderNo,
            Status = data.OrderStatus,
            PaymentStatus = data.PaymentStatus,
            TotalAmount = data.TotalAmount,
            CurrencyCode = data.CurrencyCode
        });
    }

    public async Task<ApiResponse<OrderDto>> GetMyOrderByIdAsync(OrderGetByIdRequest request, CancellationToken ct = default)
    {
        var response = await api.PostAsync<OrderGetByIdRequest, BackendOrderDetail>(ApiEndpoints.Order.GetMyOrderById, request, ct);
        return response.MapData(MapOrderDetail);
    }

    public async Task<ApiResponse<PagedResult<OrderDto>>> GetMyOrderListAsync(OrderListRequest request, CancellationToken ct = default)
    {
        var response = await api.PostAsync<OrderListRequest, BackendPaginationResponse<List<BackendOrderGrid>>>(ApiEndpoints.Order.GetMyOrderList, request, ct);
        return response.MapData(data => data.ToPagedResult(request.PageNo, request.PageSize, MapOrderGrid));
    }

    public Task<ApiResponse<object>> CancelMyOrderAsync(CancelMyOrderRequest request, CancellationToken ct = default)
        => api.PostAsync<CancelMyOrderRequest, object>(ApiEndpoints.Order.CancelMyOrder, request, ct);

    public Task<ApiResponse<object>> ReorderAsync(ReorderRequest request, CancellationToken ct = default)
        => api.PostAsync<ReorderRequest, object>(ApiEndpoints.Order.Reorder, request, ct);

    public Task<ApiResponse<object>> CreateReturnAsync(CreateOrderReturnRequest request, CancellationToken ct = default)
        => api.PostAsync<CreateOrderReturnRequest, object>(ApiEndpoints.Order.CreateReturn, request, ct);

    public Task<ApiResponse<PagedResult<OrderDto>>> GetOrderListAsync(OrderListRequest request, CancellationToken ct = default)
        => GetMyOrderListAsync(request, ct);

    public Task<ApiResponse<OrderDto>> GetOrderByIdAsync(OrderGetByIdRequest request, CancellationToken ct = default)
        => GetMyOrderByIdAsync(request, ct);

    public Task<ApiResponse<object>> UpdateOrderStatusAsync(UpdateOrderStatusRequest request, CancellationToken ct = default)
        => api.PostAsync<UpdateOrderStatusRequest, object>(ApiEndpoints.Order.UpdateOrderStatus, request, ct);

    public Task<ApiResponse<object>> CreateShipmentAsync(CreateOrderShipmentRequest request, CancellationToken ct = default)
        => api.PostAsync<CreateOrderShipmentRequest, object>(ApiEndpoints.Order.CreateShipment, request, ct);

    private static OrderDto MapOrderGrid(BackendOrderGrid order) =>
        new()
        {
            Id = order.Id,
            OrderNo = order.OrderNo,
            Status = order.OrderStatus,
            PaymentStatus = order.PaymentStatus,
            TotalQuantity = order.TotalQuantity,
            TotalAmount = order.TotalAmount,
            CurrencyCode = order.CurrencyCode,
            CreatedAt = order.PlacedAt,
            Items = new()
        };

    private static OrderDto MapOrderDetail(BackendOrderDetail order) =>
        new()
        {
            Id = order.Id,
            OrderNo = order.OrderNo,
            Status = order.OrderStatus,
            PaymentStatus = order.PaymentStatus,
            TotalQuantity = order.Items.Sum(item => item.Quantity),
            TotalAmount = order.TotalAmount,
            CurrencyCode = order.CurrencyCode,
            CreatedAt = order.PlacedAt,
            Items = order.Items.Select(item => new OrderItemDto
            {
                Id = item.Id,
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                TotalPrice = item.LineTotal
            }).ToList()
        };
}

public sealed class WishlistService(IApiClient api)
{
    public async Task<ApiResponse<List<WishlistItemDto>>> GetMyWishlistAsync(CancellationToken ct = default)
    {
        var response = await api.GetAsync<List<BackendWishlistItem>>(ApiEndpoints.Wishlist.GetMyWishlist, ct);
        return response.MapData(data => data.Select(item => new WishlistItemDto
        {
            ProductId = item.ProductId,
            ProductName = item.ProductName,
            ImageUrl = item.ImageUrl,
            Price = item.BasePrice,
            IsInStock = item.InStock
        }).ToList());
    }

    public Task<ApiResponse<object>> AddAsync(WishlistMutationRequest request, CancellationToken ct = default)
        => api.PostAsync<WishlistMutationRequest, object>(ApiEndpoints.Wishlist.Add, request, ct);

    public Task<ApiResponse<object>> RemoveAsync(WishlistRemoveRequest request, CancellationToken ct = default)
        => api.PostAsync<WishlistRemoveRequest, object>(ApiEndpoints.Wishlist.Remove, request, ct);

    public Task<ApiResponse<object>> MoveToCartAsync(WishlistRemoveRequest request, CancellationToken ct = default)
        => api.PostAsync<WishlistRemoveRequest, object>(ApiEndpoints.Wishlist.MoveToCart, request, ct);
}

public sealed class UserAccountService(IApiClient api)
{
    public async Task<ApiResponse<CustomerProfileDto>> GetMyProfileAsync(CancellationToken ct = default)
    {
        var response = await api.GetAsync<CustomerProfileDto>(ApiEndpoints.CustomerAccount.GetMyProfile, ct);
        return response;
    }

    public Task<ApiResponse<object>> UpdateMyProfileAsync(CustomerProfileUpdateRequest request, CancellationToken ct = default)
        => api.PostAsync<CustomerProfileUpdateRequest, object>(ApiEndpoints.CustomerAccount.UpdateMyProfile, request, ct);

    public Task<ApiResponse<object>> ChangePasswordAsync(ChangePasswordRequest request, CancellationToken ct = default)
        => api.PostAsync<ChangePasswordRequest, object>(ApiEndpoints.CustomerAccount.ChangePassword, request, ct);

    public async Task<ApiResponse<List<CustomerAddressDto>>> GetMyAddressesAsync(CancellationToken ct = default)
    {
        var response = await api.GetAsync<List<BackendAddress>>(ApiEndpoints.CustomerAddress.GetMyAddresses, ct);
        return response.MapData(data => data.Select(MapAddress).ToList());
    }

    public Task<ApiResponse<object>> SaveAddressAsync(CustomerAddressMutationRequest request, CancellationToken ct = default)
        => api.PostAsync<BackendAddressRequest, object>(ApiEndpoints.CustomerAddress.SaveAddress, MapAddressRequest(request), ct);

    public Task<ApiResponse<object>> DeleteAddressAsync(CustomerAddressDeleteRequest request, CancellationToken ct = default)
        => api.PostAsync<CustomerAddressDeleteRequest, object>(ApiEndpoints.CustomerAddress.DeleteAddress, request, ct);

    public Task<ApiResponse<List<NotificationDto>>> GetMyNotificationsAsync(CancellationToken ct = default)
        => api.GetAsync<List<NotificationDto>>(ApiEndpoints.Notification.GetMyNotifications, ct);

    private static BackendAddressRequest MapAddressRequest(CustomerAddressMutationRequest request) =>
        new()
        {
            Id = request.Id ?? 0,
            AddressLabel = request.IsDefault ? "Default" : "Saved Address",
            RecipientName = request.FullName,
            PhoneNumber = request.PhoneNumber,
            AddressLine1 = request.AddressLine1,
            AddressLine2 = request.AddressLine2,
            City = request.City,
            State = request.State,
            PostalCode = request.PinCode,
            CountryCode = "IN",
            IsDefaultShipping = request.IsDefault,
            IsDefaultBilling = request.IsDefault
        };

    private static CustomerAddressDto MapAddress(BackendAddress address) =>
        new()
        {
            Id = address.Id,
            FullName = address.RecipientName,
            AddressLine1 = address.AddressLine1,
            AddressLine2 = address.AddressLine2,
            City = address.City,
            State = address.State,
            PinCode = address.PostalCode,
            PhoneNumber = address.PhoneNumber,
            IsDefault = address.IsDefaultShipping || address.IsDefaultBilling
        };
}

public sealed class PaymentService(IApiClient api)
{
    public async Task<ApiResponse<PaymentOrderDto>> CreatePaymentOrderAsync(CreatePaymentOrderRequest request, CancellationToken ct = default)
    {
        var response = await api.PostAsync<CreatePaymentOrderRequest, BackendPaymentOrder>(ApiEndpoints.Payment.CreatePaymentOrder, request, ct);
        return response.MapData(data => new PaymentOrderDto
        {
            GatewayOrderId = data.ProviderOrderId,
            Amount = data.Amount,
            Currency = data.Currency,
            GatewayKey = data.KeyId,
            OrderId = data.OrderId
        });
    }

    public Task<ApiResponse<object>> VerifyPaymentAsync(VerifyPaymentRequest request, CancellationToken ct = default)
        => api.PostAsync<VerifyPaymentRequest, object>(ApiEndpoints.Payment.VerifyPayment, request, ct);

    public Task<ApiResponse<object>> RetryPaymentAsync(RetryPaymentRequest request, CancellationToken ct = default)
        => api.PostAsync<RetryPaymentRequest, object>(ApiEndpoints.Payment.RetryPayment, request, ct);
}

public sealed class AdminDashboardService(IApiClient api)
{
    public async Task<ApiResponse<AdminDashboardSummaryDto>> GetSummaryAsync(CancellationToken ct = default)
    {
        var response = await api.GetAsync<BackendAdminDashboardSummary>(ApiEndpoints.AdminDashboard.GetSummary, ct);
        return response.MapData(data => new AdminDashboardSummaryDto
        {
            TotalOrders = data.TotalOrders,
            TotalRevenue = data.TotalSales,
            TotalProducts = data.TotalProducts,
            PendingOrders = data.PendingOrders,
            LowStockProducts = data.LowStockProducts
        });
    }

    public async Task<ApiResponse<AdminDashboardOverviewDto>> GetOverviewAsync(CancellationToken ct = default)
    {
        var response = await api.GetAsync<BackendAdminDashboardOverview>(ApiEndpoints.AdminDashboard.GetDashboardOverview, ct);
        return response.MapData(data => new AdminDashboardOverviewDto
        {
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

    public Task<ApiResponse<object>> GetReportAsync(AdminDashboardReportRequest request, CancellationToken ct = default)
        => api.PostAsync<AdminDashboardReportRequest, object>(ApiEndpoints.AdminDashboard.GetDashboardReport, request, ct);
}

public sealed class AdminProductService(IApiClient api)
{
    public async Task<ApiResponse<PagedResult<AdminProductDto>>> GetProductListAsync(PaginationRequest request, CancellationToken ct = default)
    {
        var response = await api.PostAsync<PaginationRequest, BackendPaginationResponse<List<BackendProductGrid>>>(ApiEndpoints.ProductMaster.GetProductList, request, ct);
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

    public Task<ApiResponse<AdminProductDto>> GetProductByIdAsync(ProductGetByIdRequest request, CancellationToken ct = default)
        => api.PostAsync<ProductGetByIdRequest, AdminProductDto>(ApiEndpoints.ProductMaster.GetProductById, request, ct);

    public Task<ApiResponse<object>> CreateProductAsync(CreateProductRequest request, CancellationToken ct = default)
        => api.PostAsync<CreateProductRequest, object>(ApiEndpoints.ProductMaster.CreateProduct, request, ct);

    public Task<ApiResponse<object>> UpdateProductAsync(UpdateProductRequest request, CancellationToken ct = default)
        => api.PostAsync<UpdateProductRequest, object>(ApiEndpoints.ProductMaster.UpdateProduct, request, ct);

    public Task<ApiResponse<object>> UpdateProductStatusAsync(ProductStatusUpdateRequest request, CancellationToken ct = default)
        => api.PostAsync<ProductStatusUpdateRequest, object>(ApiEndpoints.ProductMaster.UpdateProductStatus, request, ct);

    public async Task<ApiResponse<PagedResult<CategoryDto>>> GetCategoryListAsync(PaginationRequest request, CancellationToken ct = default)
    {
        var response = await api.PostAsync<PaginationRequest, BackendPaginationResponse<List<BackendCategoryGrid>>>(ApiEndpoints.CategoryMaster.GetCategoryList, request, ct);
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

    public Task<ApiResponse<object>> CreateCategoryAsync(CreateCategoryRequest request, CancellationToken ct = default)
        => api.PostAsync<CreateCategoryRequest, object>(ApiEndpoints.CategoryMaster.CreateCategory, request, ct);

    public Task<ApiResponse<object>> UpdateCategoryAsync(UpdateCategoryRequest request, CancellationToken ct = default)
        => api.PostAsync<UpdateCategoryRequest, object>(ApiEndpoints.CategoryMaster.UpdateCategory, request, ct);

    public Task<ApiResponse<object>> UpdateCategoryStatusAsync(CategoryStatusUpdateRequest request, CancellationToken ct = default)
        => api.PostAsync<CategoryStatusUpdateRequest, object>(ApiEndpoints.CategoryMaster.UpdateCategoryStatus, request, ct);

    public async Task<ApiResponse<PagedResult<BrandDto>>> GetBrandListAsync(PaginationRequest request, CancellationToken ct = default)
    {
        var response = await api.PostAsync<PaginationRequest, BackendPaginationResponse<List<BackendBrandGrid>>>(ApiEndpoints.BrandMaster.GetBrandList, request, ct);
        return response.MapData(data => data.ToPagedResult(request.PageNo, request.PageSize, brand => new BrandDto
        {
            Id = brand.Id,
            BrandCode = brand.BrandCode,
            BrandName = brand.BrandName,
            IsActive = brand.IsActive
        }));
    }

    public Task<ApiResponse<object>> CreateBrandAsync(CreateBrandRequest request, CancellationToken ct = default)
        => api.PostAsync<CreateBrandRequest, object>(ApiEndpoints.BrandMaster.CreateBrand, request, ct);

    public Task<ApiResponse<object>> UpdateBrandAsync(UpdateBrandRequest request, CancellationToken ct = default)
        => api.PostAsync<UpdateBrandRequest, object>(ApiEndpoints.BrandMaster.UpdateBrand, request, ct);

    public Task<ApiResponse<object>> UpdateBrandStatusAsync(BrandStatusUpdateRequest request, CancellationToken ct = default)
        => api.PostAsync<BrandStatusUpdateRequest, object>(ApiEndpoints.BrandMaster.UpdateBrandStatus, request, ct);

    public async Task<ApiResponse<PagedResult<CouponDto>>> GetCouponListAsync(PaginationRequest request, CancellationToken ct = default)
    {
        var response = await api.PostAsync<PaginationRequest, BackendPaginationResponse<List<BackendCouponGrid>>>(ApiEndpoints.CouponMaster.GetCouponList, request, ct);
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

    public Task<ApiResponse<object>> CreateCouponAsync(CreateCouponRequest request, CancellationToken ct = default)
        => api.PostAsync<CreateCouponRequest, object>(ApiEndpoints.CouponMaster.CreateCoupon, request, ct);

    public Task<ApiResponse<object>> UpdateCouponAsync(UpdateCouponRequest request, CancellationToken ct = default)
        => api.PostAsync<UpdateCouponRequest, object>(ApiEndpoints.CouponMaster.UpdateCoupon, request, ct);

    public Task<ApiResponse<object>> UpdateCouponStatusAsync(CouponStatusUpdateRequest request, CancellationToken ct = default)
        => api.PostAsync<CouponStatusUpdateRequest, object>(ApiEndpoints.CouponMaster.UpdateCouponStatus, request, ct);
}

public sealed class AdminOrderService(IApiClient api)
{
    public async Task<ApiResponse<PagedResult<OrderDto>>> GetOrderListAsync(OrderListRequest request, CancellationToken ct = default)
    {
        var response = await api.PostAsync<OrderListRequest, BackendPaginationResponse<List<BackendOrderGrid>>>(ApiEndpoints.Order.GetOrderList, request, ct);
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
        var response = await api.PostAsync<OrderGetByIdRequest, BackendOrderDetail>(ApiEndpoints.Order.GetOrderById, request, ct);
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

    public Task<ApiResponse<object>> UpdateOrderStatusAsync(UpdateOrderStatusRequest request, CancellationToken ct = default)
        => api.PostAsync<UpdateOrderStatusRequest, object>(ApiEndpoints.Order.UpdateOrderStatus, request, ct);

    public Task<ApiResponse<object>> CreateShipmentAsync(CreateOrderShipmentRequest request, CancellationToken ct = default)
        => api.PostAsync<CreateOrderShipmentRequest, object>(ApiEndpoints.Order.CreateShipment, request, ct);
}

public sealed class AdminUserService(IApiClient api)
{
    public async Task<ApiResponse<PagedResult<AdminUserDto>>> GetUserListAsync(AdminUserListRequest request, CancellationToken ct = default)
    {
        var response = await api.PostAsync<AdminUserListRequest, BackendPaginationResponse<List<BackendAdminUserGrid>>>(ApiEndpoints.AdminUser.GetUserList, request, ct);
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
        var response = await api.PostAsync<AdminUserGetByIdRequest, BackendAdminUserGrid>(ApiEndpoints.AdminUser.GetUserById, request, ct);
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

public sealed class AdminInventoryService(IApiClient api)
{
    public Task<ApiResponse<object>> StockInAsync(InventoryAdjustmentRequest request, CancellationToken ct = default)
        => api.PostAsync<InventoryAdjustmentRequest, object>(ApiEndpoints.ProductInventory.StockIn, request, ct);

    public Task<ApiResponse<object>> StockOutAsync(InventoryAdjustmentRequest request, CancellationToken ct = default)
        => api.PostAsync<InventoryAdjustmentRequest, object>(ApiEndpoints.ProductInventory.StockOut, request, ct);

    public async Task<ApiResponse<PagedResult<InventoryTransactionDto>>> GetTransactionListAsync(ProductInventoryTransactionListRequest request, CancellationToken ct = default)
    {
        var response = await api.PostAsync<ProductInventoryTransactionListRequest, BackendPaginationResponse<List<BackendInventoryTransaction>>>(ApiEndpoints.ProductInventory.GetTransactionList, request, ct);
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
        var response = await api.PostAsync<InventoryAdminReportRequest, BackendInventoryReport>(ApiEndpoints.ProductInventory.GetInventoryAdminReport, request, ct);
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

public sealed class AdminCatalogAdminService(IApiClient api)
{
    public async Task<ApiResponse<PagedResult<AuditLogDto>>> GetAuditLogListAsync(AuditLogListRequest request, CancellationToken ct = default)
    {
        var response = await api.PostAsync<AuditLogListRequest, BackendPaginationResponse<List<BackendAuditLogGrid>>>(ApiEndpoints.AuditLog.GetAuditLogList, request, ct);
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

    public Task<ApiResponse<object>> GetDataIntegrityCheckAsync(CancellationToken ct = default)
        => api.GetAsync<object>(ApiEndpoints.AdminOperations.GetDataIntegrityCheck, ct);

    public async Task<ApiResponse<PagedResult<InvoiceAdminDto>>> GetInvoiceListAsync(PaginationRequest request, CancellationToken ct = default)
    {
        var response = await api.PostAsync<PaginationRequest, BackendPaginationResponse<List<BackendInvoiceAdminGrid>>>(ApiEndpoints.Invoice.GetInvoiceList, request, ct);
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

    public Task<ApiResponse<object>> GenerateInvoiceAsync(GenerateInvoiceRequest request, CancellationToken ct = default)
        => api.PostAsync<GenerateInvoiceRequest, object>(ApiEndpoints.Invoice.GenerateInvoice, request, ct);

    public async Task<ApiResponse<PagedResult<PaymentAdminDto>>> GetPaymentListAsync(PaginationRequest request, CancellationToken ct = default)
    {
        var response = await api.PostAsync<PaginationRequest, BackendPaginationResponse<List<BackendPaymentAdminGrid>>>(ApiEndpoints.PaymentAdmin.GetPaymentList, request, ct);
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

    public Task<ApiResponse<object>> ProcessRefundAsync(ProcessPaymentRefundRequest request, CancellationToken ct = default)
        => api.PostAsync<ProcessPaymentRefundRequest, object>(ApiEndpoints.PaymentAdmin.ProcessRefund, request, ct);
}

internal static class ServiceMappingExtensions
{
    public static ApiResponse<TOut> MapData<TIn, TOut>(this ApiResponse<TIn> response, Func<TIn, TOut> map)
        where TIn : class
        where TOut : class
    {
        return new ApiResponse<TOut>
        {
            Code = response.Code,
            Message = response.Message,
            Errors = response.Errors,
            Data = response.Data is null ? null : map(response.Data)
        };
    }

    public static PagedResult<TOut> ToPagedResult<TIn, TOut>(
        this BackendPaginationResponse<List<TIn>> page,
        int pageNo,
        int pageSize,
        Func<TIn, TOut> map)
    {
        var records = page.Records ?? new List<TIn>();
        var totalRecords = (int)page.TotalCount;
        var totalPages = pageSize <= 0 ? 1 : Math.Max(1, (int)Math.Ceiling(totalRecords / (double)pageSize));

        return new PagedResult<TOut>
        {
            Items = records.Select(map).ToList(),
            PageNo = pageNo,
            PageSize = pageSize,
            TotalRecords = totalRecords,
            TotalPages = totalPages
        };
    }
}
