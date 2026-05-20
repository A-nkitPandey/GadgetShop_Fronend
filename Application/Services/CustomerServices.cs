using GadgetShop.Application.Interfaces;
using GadgetShop.Authentication;
using GadgetShop.Constants;
using GadgetShop.Core.Mapping;
using GadgetShop.Models;
using GadgetShop.State;

namespace GadgetShop.Services;

public sealed class AuthService(
    IAuthRepository repository,
    TokenStorageService tokenStorage,
    AuthStateProvider authStateProvider,
    AuthState authState,
    CartState cartState,
    WishlistState wishlistState) : IAuthService
{
    public async Task<ApiResponse<LoginResponse>> LoginAsync(LoginRequest request, CancellationToken ct = default)
    {
        var response = await repository.LoginAsync(request, ct);
        if (response.IsSuccess && response.Data?.AccessToken is { Length: > 0 } token)
        {
            await tokenStorage.SetAccessTokenAsync(token);
            if (!string.IsNullOrWhiteSpace(response.Data.RefreshToken))
                await tokenStorage.SetRefreshTokenAsync(response.Data.RefreshToken);
            await tokenStorage.SetUserAsync(response.Data);

            var roles = new[] { response.Data.RoleCode, response.Data.RoleName }
                .Where(value => !string.IsNullOrWhiteSpace(value))
                .Select(value => value!)
                .ToList();

            authState.SetUser(response.Data.UserName ?? response.Data.FullName, roles);
            authStateProvider.NotifyAuthChanged();
        }

        return response;
    }

    public Task<ApiResponse<AuthTokenResponse>> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken ct = default) =>
        repository.RefreshTokenAsync(request, ct);

    public Task<ApiResponse<object>> RegisterAsync(CustomerRegisterRequest request, CancellationToken ct = default)
    {
        request.UserName = string.IsNullOrWhiteSpace(request.UserName) ? request.Email : request.UserName;
        return repository.RegisterAsync(request, ct);
    }

    public Task<ApiResponse<object>> ForgotPasswordAsync(ForgotPasswordRequest request, CancellationToken ct = default) =>
        repository.ForgotPasswordAsync(request, ct);

    public async Task LogoutAsync()
    {
        await tokenStorage.ClearAsync();
        authState.Clear();
        cartState.Clear();
        wishlistState.Clear();
        authStateProvider.NotifyAuthChanged();
    }

    public async Task<bool> IsAuthenticatedAsync() =>
        !string.IsNullOrWhiteSpace(await tokenStorage.GetAccessTokenAsync());

    public async Task<bool> IsAdminAsync() =>
        AppRoles.IsAdmin(await authStateProvider.GetRolesAsync());
}

public sealed class CatalogService(ICatalogRepository repository, HttpClient http) : ICatalogService, IRecommendationService
{
    private readonly Uri? _apiBaseAddress = http.BaseAddress;

    public async Task<ApiResponse<PagedResult<CustomerCatalogProductDto>>> GetProductsAsync(CustomerCatalogListRequest request, CancellationToken ct = default)
    {
        var response = await repository.GetProductsAsync(request, ct);
        return response.MapData(data => data.ToPagedResult(request.PageNo, request.PageSize, MapCatalogProduct));
    }

    public async Task<ApiResponse<CustomerCatalogProductDto>> GetProductByIdAsync(CustomerCatalogGetByIdRequest request, CancellationToken ct = default)
    {
        var response = await repository.GetProductByIdAsync(request, ct);
        return response.MapData(MapProductDetail);
    }

    public async Task<ApiResponse<List<CustomerCatalogProductDto>>> GetRecommendationsAsync(long? productId = null, int pageSize = 8, CancellationToken ct = default)
    {
        var response = await repository.GetRecommendationsAsync(new BackendRecommendationRequest { ProductId = productId, PageSize = pageSize }, ct);
        return response.MapData(data => data.Sections.SelectMany(section => section.Products).Select(MapCatalogProduct).ToList());
    }

    public async Task<ApiResponse<PagedResult<ProductReviewDto>>> GetReviewsAsync(ProductReviewListRequest request, CancellationToken ct = default)
    {
        var response = await repository.GetReviewsAsync(request, ct);
        return response.MapData(data => data.ToPagedResult(request.PageNo, request.PageSize, MapReview));
    }

    public Task<ApiResponse<object>> SaveReviewAsync(ProductReviewMutationRequest request, CancellationToken ct = default) =>
        repository.SaveReviewAsync(request, ct);

    public Task<ApiResponse<object>> DeleteReviewAsync(ProductReviewDeleteRequest request, CancellationToken ct = default) =>
        repository.DeleteReviewAsync(request, ct);

    private CustomerCatalogProductDto MapCatalogProduct(BackendCustomerCatalogListItem item) =>
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
            PrimaryImageUrl = AssetUrlResolver.Normalize(_apiBaseAddress, item.ImageUrl),
            CategoryName = item.CategoryName,
            BrandName = item.BrandName
        };

    private CustomerCatalogProductDto MapProductDetail(BackendProductDetail item) =>
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
            PrimaryImageUrl = AssetUrlResolver.Normalize(
                _apiBaseAddress,
                item.ImageUrl ?? item.GalleryImages.OrderByDescending(image => image.IsPrimary).ThenBy(image => image.DisplayOrder).Select(image => image.ImageUrl).FirstOrDefault()),
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
            GalleryImages = item.GalleryImages
                .OrderBy(image => image.DisplayOrder)
                .Select(image => AssetUrlResolver.Normalize(_apiBaseAddress, image.ImageUrl) ?? string.Empty)
                .ToList()
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

public sealed class CartService(ICartRepository repository, HttpClient http) : ICartService
{
    private readonly Uri? _apiBaseAddress = http.BaseAddress;

    public async Task<ApiResponse<CartDto>> GetMyCartAsync(CancellationToken ct = default)
    {
        var response = await repository.GetMyCartAsync(ct);
        return response.MapData(MapCart);
    }

    public Task<ApiResponse<CartDto>> AddItemAsync(AddCartItemRequest request, CancellationToken ct = default) =>
        ExecuteAndReloadCartAsync(() => repository.AddItemAsync(request, ct), ct);

    public Task<ApiResponse<CartDto>> UpdateQuantityAsync(UpdateCartItemQuantityRequest request, CancellationToken ct = default) =>
        ExecuteAndReloadCartAsync(() => repository.UpdateQuantityAsync(request, ct), ct);

    public Task<ApiResponse<CartDto>> RemoveItemAsync(RemoveCartItemRequest request, CancellationToken ct = default) =>
        ExecuteAndReloadCartAsync(() => repository.RemoveItemAsync(request, ct), ct);

    public Task<ApiResponse<CartDto>> ClearCartAsync(CancellationToken ct = default) =>
        ExecuteAndReloadCartAsync(() => repository.ClearCartAsync(ct), ct);

    public Task<ApiResponse<CartDto>> ApplyCouponAsync(ApplyCartCouponRequest request, CancellationToken ct = default) =>
        ExecuteAndReloadCartAsync(() => repository.ApplyCouponAsync(request, ct), ct);

    public Task<ApiResponse<CartDto>> RemoveCouponAsync(CancellationToken ct = default) =>
        ExecuteAndReloadCartAsync(() => repository.RemoveCouponAsync(ct), ct);

    private async Task<ApiResponse<CartDto>> ExecuteAndReloadCartAsync<TResponse>(Func<Task<ApiResponse<TResponse>>> action, CancellationToken ct)
    {
        var mutation = await action();
        if (!mutation.IsSuccess)
        {
            return new ApiResponse<CartDto> { Code = mutation.Code, Message = mutation.Message, Errors = mutation.Errors };
        }

        return await GetMyCartAsync(ct);
    }

    private CartDto MapCart(BackendCart cart) =>
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
                ImageUrl = AssetUrlResolver.Normalize(_apiBaseAddress, item.ImageUrl),
                UnitPrice = item.UnitPrice,
                Quantity = item.Quantity,
                TotalPrice = item.LineTotal,
                AvailableStock = item.AvailableQuantity
            }).ToList()
        };
}

public sealed class WishlistService(IWishlistRepository repository, HttpClient http) : IWishlistService
{
    private readonly Uri? _apiBaseAddress = http.BaseAddress;

    public async Task<ApiResponse<List<WishlistItemDto>>> GetMyWishlistAsync(CancellationToken ct = default)
    {
        var response = await repository.GetMyWishlistAsync(ct);
        return response.MapData(data => data.Select(item => new WishlistItemDto
        {
            Id = item.Id,
            ProductId = item.ProductId,
            ProductName = item.ProductName,
            ImageUrl = AssetUrlResolver.Normalize(_apiBaseAddress, item.ImageUrl),
            Price = item.BasePrice,
            IsInStock = item.InStock
        }).ToList());
    }

    public Task<ApiResponse<object>> AddAsync(WishlistMutationRequest request, CancellationToken ct = default) => repository.AddAsync(request, ct);
    public Task<ApiResponse<object>> RemoveAsync(WishlistRemoveRequest request, CancellationToken ct = default) => repository.RemoveAsync(request, ct);
    public Task<ApiResponse<object>> MoveToCartAsync(WishlistRemoveRequest request, CancellationToken ct = default) => repository.MoveToCartAsync(request, ct);
}

public sealed class OrderService(IOrderRepository repository) : IOrderService, ICheckoutService
{
    public async Task<ApiResponse<OrderDto>> PlaceOrderAsync(PlaceOrderRequest request, CancellationToken ct = default)
    {
        var response = await repository.PlaceOrderAsync(request, ct);
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
        var response = await repository.GetMyOrderByIdAsync(request, ct);
        return response.MapData(MapOrderDetail);
    }

    public async Task<ApiResponse<PagedResult<OrderDto>>> GetMyOrderListAsync(OrderListRequest request, CancellationToken ct = default)
    {
        var response = await repository.GetMyOrderListAsync(request, ct);
        return response.MapData(data => data.ToPagedResult(request.PageNo, request.PageSize, MapOrderGrid));
    }

    public Task<ApiResponse<object>> CancelMyOrderAsync(CancelMyOrderRequest request, CancellationToken ct = default) => repository.CancelMyOrderAsync(request, ct);
    public Task<ApiResponse<object>> ReorderAsync(ReorderRequest request, CancellationToken ct = default) => repository.ReorderAsync(request, ct);
    public async Task<ApiResponse<InvoicePrintDocumentDto>> GetMyInvoiceAsync(OrderGetByIdRequest request, CancellationToken ct = default)
    {
        var response = await repository.GetMyInvoiceAsync(request, ct);
        return response.MapData(MapInvoiceDocument);
    }

    public async Task<ApiResponse<OrderShipmentDto>> GetMyShipmentByOrderIdAsync(OrderShipmentGetByOrderIdRequest request, CancellationToken ct = default)
    {
        var response = await repository.GetMyShipmentByOrderIdAsync(request, ct);
        return response.MapData(MapShipment);
    }

    public async Task<ApiResponse<PagedResult<OrderReturnDto>>> GetMyReturnListAsync(MyReturnListRequest request, CancellationToken ct = default)
    {
        var response = await repository.GetMyReturnListAsync(request, ct);
        return response.MapData(data => data.ToPagedResult(request.PageNo, request.PageSize, MapReturn));
    }

    public Task<ApiResponse<object>> CreateReturnAsync(CreateOrderReturnRequest request, CancellationToken ct = default) => repository.CreateReturnAsync(request, ct);

    public async Task<ApiResponse<PagedResult<OrderDto>>> GetOrderListAsync(OrderListRequest request, CancellationToken ct = default)
    {
        var response = await repository.GetOrderListAsync(request, ct);
        return response.MapData(data => data.ToPagedResult(request.PageNo, request.PageSize, MapAdminOrderGrid));
    }

    public async Task<ApiResponse<OrderDto>> GetOrderByIdAsync(OrderGetByIdRequest request, CancellationToken ct = default)
    {
        var response = await repository.GetOrderByIdAsync(request, ct);
        return response.MapData(MapAdminOrderDetail);
    }

    public Task<ApiResponse<object>> UpdateOrderStatusAsync(UpdateOrderStatusRequest request, CancellationToken ct = default) => repository.UpdateOrderStatusAsync(request, ct);
    public Task<ApiResponse<object>> CreateShipmentAsync(CreateOrderShipmentRequest request, CancellationToken ct = default) => repository.CreateShipmentAsync(request, ct);

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

    private static OrderDto MapAdminOrderGrid(BackendOrderGrid order)
    {
        var dto = MapOrderGrid(order);
        dto.CustomerName = order.FullName;
        return dto;
    }

    private static OrderDto MapAdminOrderDetail(BackendOrderDetail order)
    {
        var dto = MapOrderDetail(order);
        dto.CustomerName = order.FullName;
        return dto;
    }

    private static InvoicePrintDocumentDto MapInvoiceDocument(BackendInvoicePrintDocument document) =>
        new()
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
        };

    private static OrderShipmentDto MapShipment(BackendOrderShipmentDetail shipment) =>
        new()
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
        };

    private static OrderReturnDto MapReturn(BackendOrderReturnGrid item) =>
        new()
        {
            Id = item.Id,
            OrderId = item.OrderId,
            OrderNo = item.OrderNo,
            ReturnNo = item.ReturnNo,
            ReturnStatus = item.ReturnStatus,
            RefundStatus = item.RefundStatus,
            RequestedAt = item.RequestedAt,
            CompletedAt = item.CompletedAt
        };
}

public sealed class UserAccountService(IUserAccountRepository repository) : IUserAccountService, IAddressService
{
    public Task<ApiResponse<CustomerProfileDto>> GetMyProfileAsync(CancellationToken ct = default) => repository.GetMyProfileAsync(ct);
    public Task<ApiResponse<object>> UpdateMyProfileAsync(CustomerProfileUpdateRequest request, CancellationToken ct = default) => repository.UpdateMyProfileAsync(request, ct);
    public Task<ApiResponse<object>> ChangePasswordAsync(ChangePasswordRequest request, CancellationToken ct = default) => repository.ChangePasswordAsync(request, ct);
    public Task<ApiResponse<object>> DeactivateMyAccountAsync(CancellationToken ct = default) => repository.DeactivateMyAccountAsync(ct);
    public Task<ApiResponse<List<NotificationDto>>> GetMyNotificationsAsync(CancellationToken ct = default) => repository.GetMyNotificationsAsync(ct);
    public Task<ApiResponse<object>> MarkNotificationAsReadAsync(long notificationId, CancellationToken ct = default) => repository.MarkNotificationAsReadAsync(notificationId, ct);
    public Task<ApiResponse<object>> RegisterDeviceAsync(RegisterPushDeviceRequest request, CancellationToken ct = default) => repository.RegisterDeviceAsync(request, ct);
    public Task<ApiResponse<object>> RemoveDeviceAsync(RemovePushDeviceRequest request, CancellationToken ct = default) => repository.RemoveDeviceAsync(request, ct);

    public async Task<ApiResponse<List<CustomerAddressDto>>> GetMyAddressesAsync(CancellationToken ct = default)
    {
        var response = await repository.GetMyAddressesAsync(ct);
        return response.MapData(data => data.Select(MapAddress).ToList());
    }

    public Task<ApiResponse<object>> SaveAddressAsync(CustomerAddressMutationRequest request, CancellationToken ct = default) =>
        repository.SaveAddressAsync(MapAddressRequest(request), ct);

    public Task<ApiResponse<object>> DeleteAddressAsync(CustomerAddressDeleteRequest request, CancellationToken ct = default) =>
        repository.DeleteAddressAsync(request, ct);

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

public sealed class SupportTicketService(ISupportTicketRepository repository) : ISupportTicketService
{
    public Task<ApiResponse<List<SupportTicketDto>>> GetMyTicketsAsync(CancellationToken ct = default) =>
        repository.GetMyTicketsAsync(ct);

    public Task<ApiResponse<SupportTicketDto>> CreateAsync(SupportTicketMutationRequest request, CancellationToken ct = default) =>
        repository.CreateAsync(request, ct);

    public Task<ApiResponse<SupportTicketDto>> ReplyAsync(SupportTicketReplyRequest request, CancellationToken ct = default) =>
        repository.ReplyAsync(request, ct);
}

public sealed class AdminSupportTicketService(ISupportTicketRepository repository) : IAdminSupportTicketService
{
    public async Task<ApiResponse<PagedResult<SupportTicketDto>>> GetTicketsAsync(AdminSupportTicketListRequest request, CancellationToken ct = default)
    {
        var response = await repository.GetAdminTicketsAsync(request, ct);
        return response.MapData(data => data.ToPagedResult(request.PageNo, request.PageSize, ticket => ticket));
    }

    public Task<ApiResponse<SupportTicketDto>> ReplyAsAdminAsync(AdminSupportTicketReplyRequest request, CancellationToken ct = default) =>
        repository.ReplyAsAdminAsync(request, ct);
}

public sealed class PaymentService(IPaymentRepository repository) : IPaymentService
{
    public async Task<ApiResponse<PaymentOrderDto>> CreatePaymentOrderAsync(CreatePaymentOrderRequest request, CancellationToken ct = default)
    {
        var response = await repository.CreatePaymentOrderAsync(request, ct);
        return response.MapData(data => new PaymentOrderDto
        {
            GatewayOrderId = data.ProviderOrderId,
            Amount = data.Amount,
            AmountInSubunits = data.AmountInSubunits,
            Currency = data.Currency,
            GatewayKey = data.KeyId,
            OrderId = data.OrderId
        });
    }

    public Task<ApiResponse<object>> VerifyPaymentAsync(VerifyPaymentRequest request, CancellationToken ct = default) => repository.VerifyPaymentAsync(request, ct);
    public async Task<ApiResponse<PaymentOrderDto>> RetryPaymentAsync(RetryPaymentRequest request, CancellationToken ct = default)
    {
        var response = await repository.RetryPaymentAsync(request, ct);
        return response.MapData(data => new PaymentOrderDto
        {
            GatewayOrderId = data.ProviderOrderId,
            Amount = data.Amount,
            AmountInSubunits = data.AmountInSubunits,
            Currency = data.Currency,
            GatewayKey = data.KeyId,
            OrderId = data.OrderId
        });
    }
}
