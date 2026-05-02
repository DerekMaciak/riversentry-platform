namespace RiverSentry.Mobile;

public static class AppConfig
{
    // Base URL for the web app
    // TODO: Uncomment localhost block and comment production line to test locally
    // #if DEBUG
    //     #if ANDROID
    //         // Android emulator uses 10.0.2.2 to reach host localhost
    //         public const string WebBaseUrl = "http://10.0.2.2:5210";
    //     #else
    //         public const string WebBaseUrl = "https://localhost:7113";
    //     #endif
    // #else
    //     public const string WebBaseUrl = "https://riversentry-web.azurewebsites.net";
    // #endif
    public const string WebBaseUrl = "https://riversentry-web.azurewebsites.net";
}
