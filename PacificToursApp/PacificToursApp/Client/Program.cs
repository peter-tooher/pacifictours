// Global using directives, these namespaces are available in all files without explicit using statements
global using PacificToursApp.Shared;
global using System.Net.Http.Json;
global using PacificToursApp.Client.Services.TourService;
global using PacificToursApp.Client.Services.HotelService;
global using PacificToursApp.Client.Services.AuthService;
global using PacificToursApp.Client.Services;
global using Microsoft.AspNetCore.Components.Authorization;

// Local using directives, these namespaces are only available in this file
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using PacificToursApp.Client;
using Blazored.LocalStorage;

// Create a WebAssemblyHostBuilder which is used to configure an application's services and start the application
var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Add the root component 'App' to the builder and specify its CSS selector
builder.RootComponents.Add<App>("#app");

// Add the 'HeadOutlet' component to the builder and specify its CSS selector
builder.RootComponents.Add<HeadOutlet>("head::after");

// Add Blazored LocalStorage service to the DI container
builder.Services.AddBlazoredLocalStorage();

// Add a scoped HttpClient to the DI container with the application's base address
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Add scoped services to the DI container
builder.Services.AddScoped<ITourService, TourService>();
builder.Services.AddScoped<IHotelService, HotelService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<BookingService>();

// Add the authorization core services to the DI container
builder.Services.AddAuthorizationCore();

// Add a custom AuthenticationStateProvider to the DI container
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

// Build and run the application
await builder.Build().RunAsync();
