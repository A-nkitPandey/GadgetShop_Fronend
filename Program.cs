using Blazored.LocalStorage;
using GadgetShop;
using GadgetShop.ApiClients;
using GadgetShop.Authentication;
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
builder.Services.AddHttpClient("GadgetApi", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"] ?? "https://localhost:7058/");
    client.Timeout = TimeSpan.FromSeconds(30);
})
.AddHttpMessageHandler<AuthHeaderHandler>();

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

// --- App Services ---
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<CatalogService>();
builder.Services.AddScoped<CartService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<WishlistService>();
builder.Services.AddScoped<UserAccountService>();
builder.Services.AddScoped<AdminProductService>();
builder.Services.AddScoped<AdminOrderService>();
builder.Services.AddScoped<AdminDashboardService>();
builder.Services.AddScoped<AdminUserService>();
builder.Services.AddScoped<AdminInventoryService>();
builder.Services.AddScoped<AdminCatalogAdminService>();
builder.Services.AddScoped<PaymentService>();

// --- State ---
builder.Services.AddScoped<CartState>();
builder.Services.AddScoped<AuthState>();

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
