using Blazored.LocalStorage;
using LogiHUB.UI;
using LogiHUB.UI.Helpers;
using LogiHUB.UI.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Blazored LocalStorage
builder.Services.AddBlazoredLocalStorage();

// AuthMessageHandler is needed by our scoped HttpClient
builder.Services.AddScoped<AuthMessageHandler>();

// Single HttpClient with AuthMessageHandler
builder.Services.AddScoped(sp =>
{
    var localStorage = sp.GetRequiredService<ILocalStorageService>();
    var handler = new AuthMessageHandler(localStorage);
    return new HttpClient(handler)
    {
        BaseAddress = new Uri("https://localhost:7047")
    };
});

// Register services (they just get HttpClient injected)
builder.Services.AddScoped<ClientService>();
builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<ShipmentService>();
builder.Services.AddScoped<InvoiceService>();

await builder.Build().RunAsync();