using Android.Webkit;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using AndroidNet = Android.Net;

namespace RiverSentry.Mobile.Platforms.Android.Handlers;

/// <summary>
/// Custom WebView handler that accepts self-signed SSL certificates in DEBUG mode.
/// </summary>
public class CustomWebViewHandler : WebViewHandler
{
    protected override void ConnectHandler(global::Android.Webkit.WebView platformView)
    {
        base.ConnectHandler(platformView);

        // Enable JavaScript (required for Blazor)
        platformView.Settings.JavaScriptEnabled = true;
        platformView.Settings.DomStorageEnabled = true;
        platformView.Settings.MixedContentMode = MixedContentHandling.AlwaysAllow;

        // Enable audio/media playback
        platformView.Settings.MediaPlaybackRequiresUserGesture = false;

        // Required for audio playback
        platformView.SetWebChromeClient(new WebChromeClient());

#if DEBUG
        // Enable WebView debugging
        global::Android.Webkit.WebView.SetWebContentsDebuggingEnabled(true);

        // Get the existing MAUI WebViewClient and wrap it
        platformView.SetWebViewClient(new TrustingWebViewClient(this));
#endif
    }

#if DEBUG
    private class TrustingWebViewClient : MauiWebViewClient
    {
        public TrustingWebViewClient(WebViewHandler handler) : base(handler)
        {
        }

        public override void OnReceivedSslError(global::Android.Webkit.WebView? view, SslErrorHandler? handler, AndroidNet.Http.SslError? error)
        {
            // Accept all certificates in debug mode
            System.Diagnostics.Debug.WriteLine($"SSL Error: {error?.PrimaryError} - Proceeding anyway");
            handler?.Proceed();
        }

        public override void OnReceivedError(global::Android.Webkit.WebView? view, IWebResourceRequest? request, WebResourceError? error)
        {
            System.Diagnostics.Debug.WriteLine($"WebView Error: {error?.Description} for {request?.Url}");
            base.OnReceivedError(view, request, error);
        }

        public override void OnPageFinished(global::Android.Webkit.WebView? view, string? url)
        {
            System.Diagnostics.Debug.WriteLine($"Page finished loading: {url}");
            base.OnPageFinished(view, url);
        }
    }
#endif
}
