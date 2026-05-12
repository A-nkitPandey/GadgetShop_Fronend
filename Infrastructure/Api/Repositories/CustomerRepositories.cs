using GadgetShop.ApiClients;
using GadgetShop.Application.Interfaces;
using GadgetShop.Constants;
using GadgetShop.Models;

namespace GadgetShop.Infrastructure.Api.Repositories;

public sealed class AuthRepository(IApiClient api) : IAuthRepository
{
    public Task<ApiResponse<LoginResponse>> LoginAsync(LoginRequest request, CancellationToken ct = default) =>
        api.PostAsync<LoginRequest, LoginResponse>(ApiEndpoints.Auth.Login, request, ct);

    public Task<ApiResponse<object>> RegisterAsync(CustomerRegisterRequest request, CancellationToken ct = default) =>
        api.PostAsync<CustomerRegisterRequest, object>(ApiEndpoints.CustomerAccount.Register, request, ct);

    public Task<ApiResponse<object>> ForgotPasswordAsync(ForgotPasswordRequest request, CancellationToken ct = default) =>
        api.PostAsync<ForgotPasswordRequest, object>(ApiEndpoints.CustomerAccount.ForgotPassword, request, ct);
}

public sealed class CatalogRepository(IApiClient api) : ICatalogRepository
{
    public Task<ApiResponse<BackendPaginationResponse<List<BackendCustomerCatalogListItem>>>> GetProductsAsync(CustomerCatalogListRequest request, CancellationToken ct = default) =>
        api.PostAsync<CustomerCatalogListRequest, BackendPaginationResponse<List<BackendCustomerCatalogListItem>>>(ApiEndpoints.CustomerCatalog.GetProductList, request, ct);

    public Task<ApiResponse<BackendProductDetail>> GetProductByIdAsync(CustomerCatalogGetByIdRequest request, CancellationToken ct = default) =>
        api.PostAsync<CustomerCatalogGetByIdRequest, BackendProductDetail>(ApiEndpoints.CustomerCatalog.GetProductById, request, ct);

    public Task<ApiResponse<BackendRecommendationResponse>> GetRecommendationsAsync(BackendRecommendationRequest request, CancellationToken ct = default) =>
        api.PostAsync<BackendRecommendationRequest, BackendRecommendationResponse>(ApiEndpoints.CustomerCatalog.GetRecommendations, request, ct);

    public Task<ApiResponse<BackendPaginationResponse<List<BackendProductReview>>>> GetReviewsAsync(ProductReviewListRequest request, CancellationToken ct = default) =>
        api.PostAsync<ProductReviewListRequest, BackendPaginationResponse<List<BackendProductReview>>>(ApiEndpoints.ProductReview.GetByProduct, request, ct);

    public Task<ApiResponse<object>> SaveReviewAsync(ProductReviewMutationRequest request, CancellationToken ct = default) =>
        api.PostAsync<ProductReviewMutationRequest, object>(ApiEndpoints.ProductReview.Save, request, ct);

    public Task<ApiResponse<object>> DeleteReviewAsync(ProductReviewDeleteRequest request, CancellationToken ct = default) =>
        api.PostAsync<ProductReviewDeleteRequest, object>(ApiEndpoints.ProductReview.Delete, request, ct);
}

public sealed class CartRepository(IApiClient api) : ICartRepository
{
    public Task<ApiResponse<BackendCart>> GetMyCartAsync(CancellationToken ct = default) =>
        api.GetAsync<BackendCart>(ApiEndpoints.Cart.GetMyCart, ct);

    public Task<ApiResponse<BackendCartMutation>> AddItemAsync(AddCartItemRequest request, CancellationToken ct = default) =>
        api.PostAsync<AddCartItemRequest, BackendCartMutation>(ApiEndpoints.Cart.AddItem, request, ct);

    public Task<ApiResponse<BackendCartMutation>> UpdateQuantityAsync(UpdateCartItemQuantityRequest request, CancellationToken ct = default) =>
        api.PostAsync<UpdateCartItemQuantityRequest, BackendCartMutation>(ApiEndpoints.Cart.UpdateItemQuantity, request, ct);

    public Task<ApiResponse<BackendCartMutation>> RemoveItemAsync(RemoveCartItemRequest request, CancellationToken ct = default) =>
        api.PostAsync<RemoveCartItemRequest, BackendCartMutation>(ApiEndpoints.Cart.RemoveItem, request, ct);

    public Task<ApiResponse<object>> ClearCartAsync(CancellationToken ct = default) =>
        api.PostEmptyAsync<object>(ApiEndpoints.Cart.ClearCart, ct);

    public Task<ApiResponse<BackendCart>> ApplyCouponAsync(ApplyCartCouponRequest request, CancellationToken ct = default) =>
        api.PostAsync<ApplyCartCouponRequest, BackendCart>(ApiEndpoints.Cart.ApplyCoupon, request, ct);

    public Task<ApiResponse<BackendCart>> RemoveCouponAsync(CancellationToken ct = default) =>
        api.PostEmptyAsync<BackendCart>(ApiEndpoints.Cart.RemoveCoupon, ct);
}

public sealed class WishlistRepository(IApiClient api) : IWishlistRepository
{
    public Task<ApiResponse<List<BackendWishlistItem>>> GetMyWishlistAsync(CancellationToken ct = default) =>
        api.GetAsync<List<BackendWishlistItem>>(ApiEndpoints.Wishlist.GetMyWishlist, ct);

    public Task<ApiResponse<object>> AddAsync(WishlistMutationRequest request, CancellationToken ct = default) =>
        api.PostAsync<WishlistMutationRequest, object>(ApiEndpoints.Wishlist.Add, request, ct);

    public Task<ApiResponse<object>> RemoveAsync(WishlistRemoveRequest request, CancellationToken ct = default) =>
        api.PostAsync<WishlistRemoveRequest, object>(ApiEndpoints.Wishlist.Remove, request, ct);

    public Task<ApiResponse<object>> MoveToCartAsync(WishlistRemoveRequest request, CancellationToken ct = default) =>
        api.PostAsync<WishlistRemoveRequest, object>(ApiEndpoints.Wishlist.MoveToCart, request, ct);
}

public sealed class OrderRepository(IApiClient api) : IOrderRepository
{
    public Task<ApiResponse<BackendPlaceOrderResponse>> PlaceOrderAsync(PlaceOrderRequest request, CancellationToken ct = default) =>
        api.PostAsync<PlaceOrderRequest, BackendPlaceOrderResponse>(ApiEndpoints.Order.PlaceOrder, request, ct);

    public Task<ApiResponse<BackendOrderDetail>> GetMyOrderByIdAsync(OrderGetByIdRequest request, CancellationToken ct = default) =>
        api.PostAsync<OrderGetByIdRequest, BackendOrderDetail>(ApiEndpoints.Order.GetMyOrderById, request, ct);

    public Task<ApiResponse<BackendPaginationResponse<List<BackendOrderGrid>>>> GetMyOrderListAsync(OrderListRequest request, CancellationToken ct = default) =>
        api.PostAsync<OrderListRequest, BackendPaginationResponse<List<BackendOrderGrid>>>(ApiEndpoints.Order.GetMyOrderList, request, ct);

    public Task<ApiResponse<object>> CancelMyOrderAsync(CancelMyOrderRequest request, CancellationToken ct = default) =>
        api.PostAsync<CancelMyOrderRequest, object>(ApiEndpoints.Order.CancelMyOrder, request, ct);

    public Task<ApiResponse<object>> ReorderAsync(ReorderRequest request, CancellationToken ct = default) =>
        api.PostAsync<ReorderRequest, object>(ApiEndpoints.Order.Reorder, request, ct);

    public Task<ApiResponse<object>> CreateReturnAsync(CreateOrderReturnRequest request, CancellationToken ct = default) =>
        api.PostAsync<CreateOrderReturnRequest, object>(ApiEndpoints.Order.CreateReturn, request, ct);

    public Task<ApiResponse<BackendPaginationResponse<List<BackendOrderGrid>>>> GetOrderListAsync(OrderListRequest request, CancellationToken ct = default) =>
        api.PostAsync<OrderListRequest, BackendPaginationResponse<List<BackendOrderGrid>>>(ApiEndpoints.Order.GetOrderList, request, ct);

    public Task<ApiResponse<BackendOrderDetail>> GetOrderByIdAsync(OrderGetByIdRequest request, CancellationToken ct = default) =>
        api.PostAsync<OrderGetByIdRequest, BackendOrderDetail>(ApiEndpoints.Order.GetOrderById, request, ct);

    public Task<ApiResponse<object>> UpdateOrderStatusAsync(UpdateOrderStatusRequest request, CancellationToken ct = default) =>
        api.PostAsync<UpdateOrderStatusRequest, object>(ApiEndpoints.Order.UpdateOrderStatus, request, ct);

    public Task<ApiResponse<object>> CreateShipmentAsync(CreateOrderShipmentRequest request, CancellationToken ct = default) =>
        api.PostAsync<CreateOrderShipmentRequest, object>(ApiEndpoints.Order.CreateShipment, request, ct);
}

public sealed class UserAccountRepository(IApiClient api) : IUserAccountRepository
{
    public Task<ApiResponse<CustomerProfileDto>> GetMyProfileAsync(CancellationToken ct = default) =>
        api.GetAsync<CustomerProfileDto>(ApiEndpoints.CustomerAccount.GetMyProfile, ct);

    public Task<ApiResponse<object>> UpdateMyProfileAsync(CustomerProfileUpdateRequest request, CancellationToken ct = default) =>
        api.PostAsync<CustomerProfileUpdateRequest, object>(ApiEndpoints.CustomerAccount.UpdateMyProfile, request, ct);

    public Task<ApiResponse<object>> ChangePasswordAsync(ChangePasswordRequest request, CancellationToken ct = default) =>
        api.PostAsync<ChangePasswordRequest, object>(ApiEndpoints.CustomerAccount.ChangePassword, request, ct);

    public Task<ApiResponse<List<BackendAddress>>> GetMyAddressesAsync(CancellationToken ct = default) =>
        api.GetAsync<List<BackendAddress>>(ApiEndpoints.CustomerAddress.GetMyAddresses, ct);

    public Task<ApiResponse<object>> SaveAddressAsync(BackendAddressRequest request, CancellationToken ct = default) =>
        api.PostAsync<BackendAddressRequest, object>(ApiEndpoints.CustomerAddress.SaveAddress, request, ct);

    public Task<ApiResponse<object>> DeleteAddressAsync(CustomerAddressDeleteRequest request, CancellationToken ct = default) =>
        api.PostAsync<CustomerAddressDeleteRequest, object>(ApiEndpoints.CustomerAddress.DeleteAddress, request, ct);

    public Task<ApiResponse<List<NotificationDto>>> GetMyNotificationsAsync(CancellationToken ct = default) =>
        api.GetAsync<List<NotificationDto>>(ApiEndpoints.Notification.GetMyNotifications, ct);
}

public sealed class PaymentRepository(IApiClient api) : IPaymentRepository
{
    public Task<ApiResponse<BackendPaymentOrder>> CreatePaymentOrderAsync(CreatePaymentOrderRequest request, CancellationToken ct = default) =>
        api.PostAsync<CreatePaymentOrderRequest, BackendPaymentOrder>(ApiEndpoints.Payment.CreatePaymentOrder, request, ct);

    public Task<ApiResponse<object>> VerifyPaymentAsync(VerifyPaymentRequest request, CancellationToken ct = default) =>
        api.PostAsync<VerifyPaymentRequest, object>(ApiEndpoints.Payment.VerifyPayment, request, ct);

    public Task<ApiResponse<object>> RetryPaymentAsync(RetryPaymentRequest request, CancellationToken ct = default) =>
        api.PostAsync<RetryPaymentRequest, object>(ApiEndpoints.Payment.RetryPayment, request, ct);
}
