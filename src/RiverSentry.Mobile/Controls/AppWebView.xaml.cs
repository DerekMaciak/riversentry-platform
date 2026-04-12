using System.Text.RegularExpressions;
using System.Windows.Input;
using RiverSentry.Mobile.Services;

namespace RiverSentry.Mobile.Controls;

public partial class AppWebView : ContentView
{
    private string? _sourceUrl;
    private readonly AlarmNotificationService _notificationService;

    public static readonly BindableProperty IsRefreshingProperty =
        BindableProperty.Create(nameof(IsRefreshing), typeof(bool), typeof(AppWebView), false);

    public static readonly BindableProperty InterceptDeviceDetailProperty =
        BindableProperty.Create(nameof(InterceptDeviceDetail), typeof(bool), typeof(AppWebView), true);

    public bool IsRefreshing
    {
        get => (bool)GetValue(IsRefreshingProperty);
        set => SetValue(IsRefreshingProperty, value);
    }

    /// <summary>
    /// When true (default), navigating to /device/{id} opens DeviceDetailPage modally.
    /// Set to false when already on DeviceDetailPage.
    /// </summary>
    public bool InterceptDeviceDetail
    {
        get => (bool)GetValue(InterceptDeviceDetailProperty);
        set => SetValue(InterceptDeviceDetailProperty, value);
    }

    public ICommand RefreshCommand => new Command(async () =>
    {
        if (_sourceUrl != null)
        {
            webView.Source = _sourceUrl;
        }
    });

    public AppWebView()
    {
        InitializeComponent();
        _notificationService = Application.Current?.Handler?.MauiContext?.Services
            .GetService<AlarmNotificationService>() ?? new AlarmNotificationService();
    }

    public void LoadUrl(string url)
    {
        _sourceUrl = url;
        loadingOverlay.IsVisible = true;
        errorOverlay.IsVisible = false;
        webView.Source = url;
    }

    private void OnNavigating(object? sender, WebNavigatingEventArgs e)
    {
        // Intercept JavaScript bridge calls
        if (e.Url.StartsWith("app://"))
        {
            e.Cancel = true;
            HandleBridgeCall(e.Url);
            return;
        }

        // Intercept device detail navigation (only when enabled)
        if (InterceptDeviceDetail)
        {
            var match = Regex.Match(e.Url, @"/device/([a-fA-F0-9-]{36})");
            if (match.Success && Guid.TryParse(match.Groups[1].Value, out var deviceId))
            {
                e.Cancel = true;
                OpenDeviceDetail(deviceId);
                return;
            }
        }

        // Show loading for new navigations (not refreshes from same URL)
        if (!e.Url.Equals(_sourceUrl, StringComparison.OrdinalIgnoreCase))
        {
            loadingOverlay.IsVisible = true;
        }
    }

    private void OnNavigated(object? sender, WebNavigatedEventArgs e)
    {
        IsRefreshing = false;
        loadingOverlay.IsVisible = false;

        if (e.Result != WebNavigationResult.Success)
        {
            errorOverlay.IsVisible = true;
            errorLabel.Text = e.Result switch
            {
                WebNavigationResult.Timeout => "Connection timed out",
                WebNavigationResult.Cancel => "Navigation cancelled",
                _ => "Unable to connect"
            };
        }
        else
        {
            errorOverlay.IsVisible = false;
        }
    }

    private void OnRetryClicked(object? sender, EventArgs e)
    {
        if (_sourceUrl != null)
        {
            errorOverlay.IsVisible = false;
            loadingOverlay.IsVisible = true;
            webView.Source = _sourceUrl;
        }
    }

    private async void OpenDeviceDetail(Guid deviceId)
    {
        var currentPage = Application.Current?.Windows.FirstOrDefault()?.Page;
        if (currentPage != null)
        {
            await currentPage.Navigation.PushModalAsync(
                new Pages.DeviceDetailPage(deviceId));
        }
    }

    private async void HandleBridgeCall(string url)
    {
        // Parse: app://action/type?params
        var uri = new Uri(url);
        var action = uri.Host;
        var type = uri.AbsolutePath.TrimStart('/');
        var query = System.Web.HttpUtility.ParseQueryString(uri.Query);

        switch (action)
        {
            case "close":
                await CloseCurrentModal();
                break;
            case "notification":
                await HandleNotification(type, query["device"] ?? "Device");
                break;
        }
    }

    private async Task CloseCurrentModal()
    {
        var currentPage = Application.Current?.Windows.FirstOrDefault()?.Page;
        if (currentPage?.Navigation.ModalStack.Count > 0)
        {
            await currentPage.Navigation.PopModalAsync();
        }
    }

    private async Task HandleNotification(string type, string deviceName)
    {
        switch (type)
        {
            case "water":
                await _notificationService.SendWaterAlarmAsync(deviceName);
                break;
            case "upstream":
                await _notificationService.SendUpstreamAlarmAsync(deviceName);
                break;
            case "test":
                await _notificationService.SendTestAlarmAsync();
                break;
        }
    }
}
