using Foundation;
using Microsoft.Maui.Handlers;
using WebKit;

namespace RiverSentry.Mobile.Platforms.iOS.Handlers;

/// <summary>
/// Custom WebView handler that accepts self-signed SSL certificates in DEBUG mode.
/// </summary>
public class CustomWebViewHandler : WebViewHandler
{
    protected override void ConnectHandler(WKWebView platformView)
    {
        base.ConnectHandler(platformView);

#if DEBUG
        platformView.NavigationDelegate = new TrustingNavigationDelegate(platformView.NavigationDelegate);
#endif
    }

#if DEBUG
    private class TrustingNavigationDelegate : NSObject, IWKNavigationDelegate
    {
        private readonly IWKNavigationDelegate? _innerDelegate;

        public TrustingNavigationDelegate(IWKNavigationDelegate? innerDelegate)
        {
            _innerDelegate = innerDelegate;
        }

        [Export("webView:didReceiveAuthenticationChallenge:completionHandler:")]
        public void DidReceiveAuthenticationChallenge(
            WKWebView webView,
            NSUrlAuthenticationChallenge challenge,
            Action<NSUrlSessionAuthChallengeDisposition, NSUrlCredential?> completionHandler)
        {
            if (challenge.ProtectionSpace.AuthenticationMethod == NSUrlProtectionSpace.AuthenticationMethodServerTrust)
            {
                var trust = challenge.ProtectionSpace.ServerSecTrust;
                if (trust != null)
                {
                    completionHandler(
                        NSUrlSessionAuthChallengeDisposition.UseCredential,
                        new NSUrlCredential(trust));
                    return;
                }
            }

            completionHandler(NSUrlSessionAuthChallengeDisposition.PerformDefaultHandling, null);
        }
    }
#endif
}
