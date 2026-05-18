using Blazored.LocalStorage;
using GadgetShop.ApiClients;
using GadgetShop.Application.Interfaces;
using GadgetShop.Authentication;
using GadgetShop.Infrastructure.Api.Repositories;
using GadgetShop.Services;
using GadgetShop.State;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor.Services;

namespace GadgetShop.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGadgetShopApiClients(
        this IServiceCollection services,
        IConfiguration configuration,
        string hostBaseAddress)
    {
        var apiBaseAddress = ResolveApiBaseAddress(configuration, hostBaseAddress);
        var timeout = TimeSpan.FromSeconds(configuration.GetValue<int?>("Api:TimeoutSeconds") ?? 30);

        services.AddTransient<AuthHeaderHandler>();
        services.AddScoped<TokenRefreshCoordinator>();

        services.AddHttpClient("GadgetApi", client =>
        {
            client.BaseAddress = apiBaseAddress;
            client.Timeout = timeout;
        }).AddHttpMessageHandler<AuthHeaderHandler>();

        services.AddHttpClient("GadgetApiNoAuth", client =>
        {
            client.BaseAddress = apiBaseAddress;
            client.Timeout = timeout;
        });

        services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("GadgetApi"));
        services.AddScoped<IApiClient, ApiClient>();
        return services;
    }

    public static IServiceCollection AddGadgetShopAuthentication(this IServiceCollection services)
    {
        services.AddBlazoredLocalStorage();
        services.AddAuthorizationCore();
        services.AddScoped<TokenStorageService>();
        services.AddScoped<AuthStateProvider>();
        services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<AuthStateProvider>());
        return services;
    }

    public static IServiceCollection AddGadgetShopRepositories(this IServiceCollection services)
    {
        services.AddScoped<IAuthRepository, AuthRepository>();
        services.AddScoped<ICatalogRepository, CatalogRepository>();
        services.AddScoped<ICartRepository, CartRepository>();
        services.AddScoped<IWishlistRepository, WishlistRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IUserAccountRepository, UserAccountRepository>();
        services.AddScoped<ISupportTicketRepository, SupportTicketRepository>();
        services.AddScoped<IPaymentRepository, PaymentRepository>();
        services.AddScoped<IAdminDashboardRepository, AdminDashboardRepository>();
        services.AddScoped<IAdminProductRepository, AdminProductRepository>();
        services.AddScoped<IAdminVariantRepository, AdminVariantRepository>();
        services.AddScoped<IAdminAttributeRepository, AdminAttributeRepository>();
        services.AddScoped<IRolePermissionRepository, RolePermissionRepository>();
        services.AddScoped<IAdminArchiveRepository, AdminArchiveRepository>();
        services.AddScoped<IAdminUserRepository, AdminUserRepository>();
        services.AddScoped<IAdminInventoryRepository, AdminInventoryRepository>();
        services.AddScoped<IAdminOperationsRepository, AdminOperationsRepository>();
        return services;
    }

    public static IServiceCollection AddGadgetShopApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<AuthService>();
        services.AddScoped<IAuthService>(sp => sp.GetRequiredService<AuthService>());

        services.AddScoped<CatalogService>();
        services.AddScoped<ICatalogService>(sp => sp.GetRequiredService<CatalogService>());
        services.AddScoped<IRecommendationService>(sp => sp.GetRequiredService<CatalogService>());

        services.AddScoped<CartService>();
        services.AddScoped<ICartService>(sp => sp.GetRequiredService<CartService>());

        services.AddScoped<OrderService>();
        services.AddScoped<IOrderService>(sp => sp.GetRequiredService<OrderService>());
        services.AddScoped<ICheckoutService>(sp => sp.GetRequiredService<OrderService>());

        services.AddScoped<WishlistService>();
        services.AddScoped<IWishlistService>(sp => sp.GetRequiredService<WishlistService>());

        services.AddScoped<UserAccountService>();
        services.AddScoped<IUserAccountService>(sp => sp.GetRequiredService<UserAccountService>());
        services.AddScoped<IAddressService>(sp => sp.GetRequiredService<UserAccountService>());

        services.AddScoped<SupportTicketService>();
        services.AddScoped<ISupportTicketService>(sp => sp.GetRequiredService<SupportTicketService>());

        services.AddScoped<AdminProductService>();
        services.AddScoped<IAdminProductService>(sp => sp.GetRequiredService<AdminProductService>());
        services.AddScoped<IAdminCategoryService>(sp => sp.GetRequiredService<AdminProductService>());
        services.AddScoped<IAdminBrandService>(sp => sp.GetRequiredService<AdminProductService>());
        services.AddScoped<IAdminCouponService>(sp => sp.GetRequiredService<AdminProductService>());

        services.AddScoped<AdminOrderService>();
        services.AddScoped<IAdminOrderService>(sp => sp.GetRequiredService<AdminOrderService>());

        services.AddScoped<AdminVariantService>();
        services.AddScoped<IAdminVariantService>(sp => sp.GetRequiredService<AdminVariantService>());

        services.AddScoped<AdminAttributeService>();
        services.AddScoped<IAdminAttributeService>(sp => sp.GetRequiredService<AdminAttributeService>());

        services.AddScoped<AdminDashboardService>();
        services.AddScoped<IAdminDashboardService>(sp => sp.GetRequiredService<AdminDashboardService>());

        services.AddScoped<AdminUserService>();
        services.AddScoped<IAdminUserService>(sp => sp.GetRequiredService<AdminUserService>());

        services.AddScoped<AdminInventoryService>();
        services.AddScoped<IAdminInventoryService>(sp => sp.GetRequiredService<AdminInventoryService>());

        services.AddScoped<AdminCatalogAdminService>();
        services.AddScoped<IAdminAuditLogService>(sp => sp.GetRequiredService<AdminCatalogAdminService>());
        services.AddScoped<IAdminInvoiceService>(sp => sp.GetRequiredService<AdminCatalogAdminService>());
        services.AddScoped<IAdminPaymentService>(sp => sp.GetRequiredService<AdminCatalogAdminService>());
        services.AddScoped<IAdminOperationsService>(sp => sp.GetRequiredService<AdminCatalogAdminService>());

        services.AddScoped<RolePermissionService>();
        services.AddScoped<IRolePermissionService>(sp => sp.GetRequiredService<RolePermissionService>());

        services.AddScoped<AdminArchiveService>();
        services.AddScoped<IAdminArchiveService>(sp => sp.GetRequiredService<AdminArchiveService>());

        services.AddScoped<PaymentService>();
        services.AddScoped<IPaymentService>(sp => sp.GetRequiredService<PaymentService>());

        return services;
    }

    public static IServiceCollection AddGadgetShopState(this IServiceCollection services)
    {
        services.AddScoped<CartState>();
        services.AddScoped<AuthState>();
        services.AddScoped<WishlistState>();
        services.AddScoped<CheckoutState>();
        services.AddScoped<UiState>();
        return services;
    }

    public static IServiceCollection AddGadgetShopUi(this IServiceCollection services)
    {
        services.AddMudServices(config =>
        {
            config.SnackbarConfiguration.PositionClass = MudBlazor.Defaults.Classes.Position.BottomRight;
            config.SnackbarConfiguration.ShowTransitionDuration = 300;
            config.SnackbarConfiguration.HideTransitionDuration = 300;
            config.SnackbarConfiguration.SnackbarVariant = MudBlazor.Variant.Filled;
            config.SnackbarConfiguration.MaxDisplayedSnackbars = 4;
        });

        return services;
    }

    private static Uri ResolveApiBaseAddress(IConfiguration configuration, string hostBaseAddress)
    {
        var configuredBaseUrl = configuration["Api:BaseUrl"] ?? configuration["ApiBaseUrl"] ?? "/";
        return new Uri(new Uri(hostBaseAddress), configuredBaseUrl);
    }
}
