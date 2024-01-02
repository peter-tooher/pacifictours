global using PacificToursApp.Shared;
global using System.Net.Http.Json;
global using PacificToursApp.Client.Services.TourService;
global using PacificToursApp.Client.Services.HotelService;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using PacificToursApp.Client;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<ITourService, TourService>();
builder.Services.AddScoped<IHotelService, HotelService>();

await builder.Build().RunAsync();