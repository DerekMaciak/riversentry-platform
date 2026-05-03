using Microsoft.AspNetCore.ResponseCompression;
using RiverSentry.UI.Shared.Services;
using RiverSentry.Web.Components;
using RiverSentry.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Razor Components with tuned SignalR circuit settings for mobile stability
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents(options =>
    {
        // Keep disconnected circuits alive longer so mobile can reconnect after sleep/background
        options.DisconnectedCircuitRetentionPeriod = TimeSpan.FromMinutes(10);
        options.DisconnectedCircuitMaxRetained = 50;
    });

// Tune the underlying SignalR transport — longer keepalive reduces unnecessary reconnects
builder.Services.AddSignalR(options =>
{
    options.KeepAliveInterval = TimeSpan.FromSeconds(30);      // Default 15s — too aggressive for mobile
    options.ClientTimeoutInterval = TimeSpan.FromMinutes(2);   // Default 30s — way too short for mobile
    options.HandshakeTimeout = TimeSpan.FromSeconds(30);       // Default 15s
    options.MaximumParallelInvocationsPerClient = 2;
});

// API client — calls the River Sentry API
var apiBaseUrl = builder.Configuration["Api:BaseUrl"] ?? "http://localhost:5217";
builder.Services.AddHttpClient<IRiverSentryApiClient, RiverSentryApiClient>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
});

// Audio service (web uses JS interop)
builder.Services.AddScoped<IAlarmAudioService, WebAlarmAudioService>();

// Health checks — used by Azure Always On and external monitors to keep the app warm
builder.Services.AddHealthChecks();

// Response compression — faster page loads on mobile networks
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(["application/octet-stream"]);
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseResponseCompression();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapHealthChecks("/health");
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddAdditionalAssemblies(typeof(RiverSentry.UI.Shared._Imports).Assembly);

app.Run();
