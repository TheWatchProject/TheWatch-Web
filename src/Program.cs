using TheWatch.ServiceDefaults;
using Radzen;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TheWatch.Web.Admin;
using TheWatch.UI.RazorComponents;
using TheWatch.Contracts.Features;

var builder = WebApplication.CreateBuilder(args);

// Service defaults (Aspire)
builder.AddServiceDefaults();

// Add services
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddRadzenComponents();

// JWT Authentication configuration
var jwtKey = builder.Configuration["Jwt:Key"] ?? "SuperSecretKey123!";
var issuer = builder.Configuration["Jwt:Issuer"] ?? "TheWatch";
var audience = builder.Configuration["Jwt:Audience"] ?? "TheWatchClients";

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
    options.AddPolicy("Responder", policy => policy.RequireRole("Responder"));
});

// SignalR and generated feature evaluation services
builder.Services.AddSignalR();
var featureEvaluator = new InMemoryFeatureEvaluator(
    builder.Configuration.GetSection("FeatureFlags").GetChildren().Select(flag =>
        new FeatureDefinition(flag.Key, bool.TryParse(flag.Value, out var enabled) && enabled)));
builder.Services.AddSingleton(featureEvaluator);
builder.Services.AddSingleton<IFeatureEvaluator>(featureEvaluator);
builder.Services.AddSingleton<IMutableFeatureRegistry>(featureEvaluator);

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapDefaultEndpoints();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

// Map SignalR hubs (placeholders)
// app.MapHub<EmergencyRealTimeHub>("/hubs/emergency");
// app.MapHub<VoiceRadioStreamHub>("/hubs/voice");

app.Run();
