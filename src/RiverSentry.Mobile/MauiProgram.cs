using Microsoft.Extensions.Logging;
using RiverSentry.Mobile.Pages;
using RiverSentry.Mobile.Services;
using RiverSentry.UI.Shared.Services;

namespace RiverSentry.Mobile;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiMaps() // Enable native maps
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            })
            .ConfigureMauiHandlers(handlers =>
            {
#if ANDROID
                handlers.AddHandler<Microsoft.Maui.Controls.Maps.Map, RiverSentry.Mobile.Platforms.Android.Handlers.CustomMapHandler>();
#endif
            });

        builder.Services.AddMauiBlazorWebView();

        // API client with HTTPS
        // Use 10.0.2.2 for Android emulator, localhost for Windows
#if ANDROID
        var apiBaseUrl = "https://10.0.2.2:7010";
#else
        var apiBaseUrl = "https://localhost:7010";
#endif
        builder.Services.AddHttpClient<IRiverSentryApiClient, RiverSentryApiClient>(client =>
        {
            client.BaseAddress = new Uri(apiBaseUrl);
        })
        .ConfigurePrimaryHttpMessageHandler(() =>
        {
            var handler = new HttpClientHandler();
#if DEBUG
            // Bypass SSL certificate validation in development
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
#endif
            return handler;
        });

        // Device service for map
        builder.Services.AddTransient<IDeviceService, ApiDeviceService>();

        // Audio service (native mobile playback)
        builder.Services.AddSingleton<IAlarmAudioService, MobileAlarmAudioService>();

        // Pages
        builder.Services.AddTransient<MapPage>();
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<DevicesPage>();
        builder.Services.AddTransient<AlertsPage>();
        builder.Services.AddTransient<LocationsPage>();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
