using LogiHUB.UI;
using LogiHUB.UI.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("https://localhost:7047")
});

// Register services
builder.Services.AddScoped<ShipmentService>();
builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<InvoiceService>();

await builder.Build().RunAsync();
