using Microsoft.Extensions.Logging;
using Plugin.LocalNotification;
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
            .UseMauiMaps()
            .UseLocalNotification()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            })
            .ConfigureMauiHandlers(handlers =>
            {
#if ANDROID
                handlers.AddHandler<Microsoft.Maui.Controls.Maps.Map, RiverSentry.Mobile.Platforms.Android.Handlers.CustomMapHandler>();
                handlers.AddHandler<WebView, RiverSentry.Mobile.Platforms.Android.Handlers.CustomWebViewHandler>();
#elif IOS
                handlers.AddHandler<WebView, RiverSentry.Mobile.Platforms.iOS.Handlers.CustomWebViewHandler>();
#endif
            });

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

        // Device service for native map
        builder.Services.AddTransient<IDeviceService, ApiDeviceService>();

        // Alarm notification service
        builder.Services.AddSingleton<AlarmNotificationService>();

        // Pages - Singleton to keep state when navigating away
        builder.Services.AddSingleton<MapPage>();

        // AppShell needs MapPage injected
        builder.Services.AddSingleton<AppShell>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
