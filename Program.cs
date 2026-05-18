using GadgetShop;
using GadgetShop.Infrastructure.DependencyInjection;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddGadgetShopApiClients(builder.Configuration, builder.HostEnvironment.BaseAddress);
builder.Services.AddGadgetShopAuthentication();
builder.Services.AddGadgetShopRepositories();
builder.Services.AddGadgetShopApplicationServices();
builder.Services.AddGadgetShopState();
builder.Services.AddGadgetShopUi();

builder.Logging.SetMinimumLevel(
    builder.HostEnvironment.IsDevelopment() ? LogLevel.Information : LogLevel.Warning);

await builder.Build().RunAsync();
