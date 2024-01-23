global using PacificToursApp.Shared;
global using System.Net.Http.Json;
global using PacificToursApp.Client.Services.TourService;
global using PacificToursApp.Client.Services.HotelService;
global using PacificToursApp.Client.Services.AuthService;
global using PacificToursApp.Client.Services;
global using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using PacificToursApp.Client;
using Blazored.LocalStorage;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<ITourService, TourService>();
builder.Services.AddScoped<IHotelService, HotelService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<BookingService>();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

await builder.Build().RunAsync();
