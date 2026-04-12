namespace RiverSentry.Mobile;

public static class AppConfig
{
    // Base URL for the web app
#if DEBUG
    #if ANDROID
        // Android emulator uses 10.0.2.2 to reach host localhost
        // Use HTTP for dev (WebView has issues with self-signed HTTPS certs)
        public const string WebBaseUrl = "http://10.0.2.2:5210";
    #else
        public const string WebBaseUrl = "https://localhost:7113";
    #endif
#else
    // Production URL - update this when deploying
    public const string WebBaseUrl = "https://app.riversentry.com";
#endif
}
