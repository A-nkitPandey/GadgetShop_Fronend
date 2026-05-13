using Blazored.LocalStorage;
using GadgetShop;
using GadgetShop.ApiClients;
using GadgetShop.Application.Interfaces;
using GadgetShop.Authentication;
using GadgetShop.Infrastructure.Api.Repositories;
using GadgetShop.Services;
using GadgetShop.State;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// --- HTTP Client with Auth Handler ---
builder.Services.AddTransient<AuthHeaderHandler>();
builder.Services.AddScoped<TokenRefreshCoordinator>();
builder.Services.AddHttpClient("GadgetApi", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"] ?? "https://localhost:7058/");
    client.Timeout = TimeSpan.FromSeconds(30);
})
.AddHttpMessageHandler<AuthHeaderHandler>();

builder.Services.AddHttpClient("GadgetApiNoAuth", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"] ?? "https://localhost:7058/");
    client.Timeout = TimeSpan.FromSeconds(30);
});

builder.Services.AddScoped(sp =>
    sp.GetRequiredService<IHttpClientFactory>().CreateClient("GadgetApi"));

// --- Auth ---
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<TokenStorageService>();
builder.Services.AddScoped<AuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp =>
    sp.GetRequiredService<AuthStateProvider>());

// --- Core API Client ---
builder.Services.AddScoped<IApiClient, ApiClient>();

// --- Repositories ---
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<ICatalogRepository, CatalogRepository>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<IWishlistRepository, WishlistRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IUserAccountRepository, UserAccountRepository>();
builder.Services.AddScoped<ISupportTicketRepository, SupportTicketRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IAdminDashboardRepository, AdminDashboardRepository>();
builder.Services.AddScoped<IAdminProductRepository, AdminProductRepository>();
builder.Services.AddScoped<IAdminVariantRepository, AdminVariantRepository>();
builder.Services.AddScoped<IAdminAttributeRepository, AdminAttributeRepository>();
builder.Services.AddScoped<IRolePermissionRepository, RolePermissionRepository>();
builder.Services.AddScoped<IAdminArchiveRepository, AdminArchiveRepository>();
builder.Services.AddScoped<IAdminUserRepository, AdminUserRepository>();
builder.Services.AddScoped<IAdminInventoryRepository, AdminInventoryRepository>();
builder.Services.AddScoped<IAdminOperationsRepository, AdminOperationsRepository>();

// --- App Services ---
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<IAuthService>(sp => sp.GetRequiredService<AuthService>());

builder.Services.AddScoped<CatalogService>();
builder.Services.AddScoped<ICatalogService>(sp => sp.GetRequiredService<CatalogService>());
builder.Services.AddScoped<IRecommendationService>(sp => sp.GetRequiredService<CatalogService>());

builder.Services.AddScoped<CartService>();
builder.Services.AddScoped<ICartService>(sp => sp.GetRequiredService<CartService>());

builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<IOrderService>(sp => sp.GetRequiredService<OrderService>());
builder.Services.AddScoped<ICheckoutService>(sp => sp.GetRequiredService<OrderService>());

builder.Services.AddScoped<WishlistService>();
builder.Services.AddScoped<IWishlistService>(sp => sp.GetRequiredService<WishlistService>());

builder.Services.AddScoped<UserAccountService>();
builder.Services.AddScoped<IUserAccountService>(sp => sp.GetRequiredService<UserAccountService>());
builder.Services.AddScoped<IAddressService>(sp => sp.GetRequiredService<UserAccountService>());

builder.Services.AddScoped<SupportTicketService>();
builder.Services.AddScoped<ISupportTicketService>(sp => sp.GetRequiredService<SupportTicketService>());

builder.Services.AddScoped<AdminProductService>();
builder.Services.AddScoped<IAdminProductService>(sp => sp.GetRequiredService<AdminProductService>());
builder.Services.AddScoped<IAdminCategoryService>(sp => sp.GetRequiredService<AdminProductService>());
builder.Services.AddScoped<IAdminBrandService>(sp => sp.GetRequiredService<AdminProductService>());
builder.Services.AddScoped<IAdminCouponService>(sp => sp.GetRequiredService<AdminProductService>());

builder.Services.AddScoped<AdminOrderService>();
builder.Services.AddScoped<IAdminOrderService>(sp => sp.GetRequiredService<AdminOrderService>());

builder.Services.AddScoped<AdminVariantService>();
builder.Services.AddScoped<IAdminVariantService>(sp => sp.GetRequiredService<AdminVariantService>());

builder.Services.AddScoped<AdminAttributeService>();
builder.Services.AddScoped<IAdminAttributeService>(sp => sp.GetRequiredService<AdminAttributeService>());

builder.Services.AddScoped<AdminDashboardService>();
builder.Services.AddScoped<IAdminDashboardService>(sp => sp.GetRequiredService<AdminDashboardService>());

builder.Services.AddScoped<AdminUserService>();
builder.Services.AddScoped<IAdminUserService>(sp => sp.GetRequiredService<AdminUserService>());

builder.Services.AddScoped<AdminInventoryService>();
builder.Services.AddScoped<IAdminInventoryService>(sp => sp.GetRequiredService<AdminInventoryService>());

builder.Services.AddScoped<AdminCatalogAdminService>();
builder.Services.AddScoped<IAdminAuditLogService>(sp => sp.GetRequiredService<AdminCatalogAdminService>());
builder.Services.AddScoped<IAdminInvoiceService>(sp => sp.GetRequiredService<AdminCatalogAdminService>());
builder.Services.AddScoped<IAdminPaymentService>(sp => sp.GetRequiredService<AdminCatalogAdminService>());
builder.Services.AddScoped<IAdminOperationsService>(sp => sp.GetRequiredService<AdminCatalogAdminService>());

builder.Services.AddScoped<RolePermissionService>();
builder.Services.AddScoped<IRolePermissionService>(sp => sp.GetRequiredService<RolePermissionService>());

builder.Services.AddScoped<AdminArchiveService>();
builder.Services.AddScoped<IAdminArchiveService>(sp => sp.GetRequiredService<AdminArchiveService>());

builder.Services.AddScoped<PaymentService>();
builder.Services.AddScoped<IPaymentService>(sp => sp.GetRequiredService<PaymentService>());

// --- State ---
builder.Services.AddScoped<CartState>();
builder.Services.AddScoped<AuthState>();
builder.Services.AddScoped<WishlistState>();
builder.Services.AddScoped<CheckoutState>();
builder.Services.AddScoped<UiState>();

// --- MudBlazor ---
builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = MudBlazor.Defaults.Classes.Position.BottomRight;
    config.SnackbarConfiguration.ShowTransitionDuration = 300;
    config.SnackbarConfiguration.HideTransitionDuration = 300;
    config.SnackbarConfiguration.SnackbarVariant = MudBlazor.Variant.Filled;
    config.SnackbarConfiguration.MaxDisplayedSnackbars = 4;
});
builder.Logging.SetMinimumLevel(LogLevel.Debug);
await builder.Build().RunAsync();
