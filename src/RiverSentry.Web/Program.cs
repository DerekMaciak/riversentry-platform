using RiverSentry.UI.Shared.Services;
using RiverSentry.Web.Components;
using RiverSentry.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Razor Components
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// API client — calls the River Sentry API
var apiBaseUrl = builder.Configuration["Api:BaseUrl"] ?? "http://localhost:5217";
builder.Services.AddHttpClient<IRiverSentryApiClient, RiverSentryApiClient>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
});

// Audio service (web uses JS interop)
builder.Services.AddScoped<IAlarmAudioService, WebAlarmAudioService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddAdditionalAssemblies(typeof(RiverSentry.UI.Shared._Imports).Assembly);

app.Run();
