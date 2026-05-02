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

        // Allow inline media playback and auto-play (matches Android MediaPlaybackRequiresUserGesture=false)
        platformView.Configuration.AllowsInlineMediaPlayback = true;
        platformView.Configuration.MediaTypesRequiringUserActionForPlayback = WebKit.WKAudiovisualMediaTypes.None;

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
            Action<NSUrlSessionAuthChallengeDisposition, NSUrlCredential> completionHandler)
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

            completionHandler(NSUrlSessionAuthChallengeDisposition.PerformDefaultHandling, null!);
        }

        // Forward navigation lifecycle events to MAUI's inner delegate so Navigated fires
        [Export("webView:didFinishNavigation:")]
        public void DidFinishNavigation(WKWebView webView, WKNavigation navigation)
        {
            if (_innerDelegate is NSObject inner && inner.RespondsToSelector(new ObjCRuntime.Selector("webView:didFinishNavigation:")))
            {
                _innerDelegate.DidFinishNavigation(webView, navigation);
            }
        }

        [Export("webView:didFailNavigation:withError:")]
        public void DidFailNavigation(WKWebView webView, WKNavigation navigation, NSError error)
        {
            if (_innerDelegate is NSObject inner && inner.RespondsToSelector(new ObjCRuntime.Selector("webView:didFailNavigation:withError:")))
            {
                _innerDelegate.DidFailNavigation(webView, navigation, error);
            }
        }

        [Export("webView:didFailProvisionalNavigation:withError:")]
        public void DidFailProvisionalNavigation(WKWebView webView, WKNavigation navigation, NSError error)
        {
            if (_innerDelegate is NSObject inner && inner.RespondsToSelector(new ObjCRuntime.Selector("webView:didFailProvisionalNavigation:withError:")))
            {
                _innerDelegate.DidFailProvisionalNavigation(webView, navigation, error);
            }
        }

        [Export("webView:didStartProvisionalNavigation:")]
        public void DidStartProvisionalNavigation(WKWebView webView, WKNavigation navigation)
        {
            if (_innerDelegate is NSObject inner && inner.RespondsToSelector(new ObjCRuntime.Selector("webView:didStartProvisionalNavigation:")))
            {
                _innerDelegate.DidStartProvisionalNavigation(webView, navigation);
            }
        }

        [Export("webView:decidePolicyForNavigationAction:decisionHandler:")]
        public void DecidePolicy(WKWebView webView, WKNavigationAction navigationAction, Action<WKNavigationActionPolicy> decisionHandler)
        {
            if (_innerDelegate is NSObject inner && inner.RespondsToSelector(new ObjCRuntime.Selector("webView:decidePolicyForNavigationAction:decisionHandler:")))
            {
                _innerDelegate.DecidePolicy(webView, navigationAction, decisionHandler);
            }
            else
            {
                decisionHandler(WKNavigationActionPolicy.Allow);
            }
        }
    }
#endif
}
