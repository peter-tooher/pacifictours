// Global using directives. These namespaces are available in all the files in the project.
global using PacificToursApp.Shared;
global using Microsoft.EntityFrameworkCore;
global using PacificToursApp.Server.Data;
global using PacificToursApp.Server.Services.HotelService;
global using PacificToursApp.Server.Services.TourService;
global using PacificToursApp.Server.Services.AuthService;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

// Create a new web application.
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add a DbContext to the service container and configure it to use SQL Server with a connection string from the configuration.
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Add MVC services to the service container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Add services to enable API explorer and Swagger.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add a hosted service for cleaning up bookings and scoped services for hotel, tour, and authentication.
builder.Services.AddHostedService<BookingCleanupService>();
builder.Services.AddScoped<IHotelService, HotelService>();
builder.Services.AddScoped<ITourService, TourService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Add authentication services to the service container and configure JWT bearer authentication.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

// Build the application.
var app = builder.Build();

// Enable Swagger UI.
app.UseSwaggerUI();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Enable debugging for WebAssembly in development.
    app.UseWebAssemblyDebugging();
}
else
{
    // Use an error handler page in production.
    app.UseExceptionHandler("/Error");
    // Use HSTS in production.
    app.UseHsts();
}

// Enable Swagger.
app.UseSwagger();

// Redirect HTTP requests to HTTPS.
app.UseHttpsRedirection();

// Serve Blazor WebAssembly files.
app.UseBlazorFrameworkFiles();

// Serve static files.
app.UseStaticFiles();

// Use routing.
app.UseRouting();

// Use authentication and authorization.
app.UseAuthentication();
app.UseAuthorization();

// Map Razor pages and controllers.
app.MapRazorPages();
app.MapControllers();

// Map a fallback file for SPA routing.
app.MapFallbackToFile("index.html");

// Run the application.
app.Run();