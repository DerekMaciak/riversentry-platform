# Push Notifications Implementation Plan

## Overview

Send flood alert notifications to mobile phones even when the app is closed, triggered from an admin page on the website. Uses **Azure Notification Hubs** (free tier: 500 devices, 1M pushes/month) to abstract FCM (Android) and APNs (iOS).

## Architecture

```
Website /admin page → API → Azure Notification Hubs → FCM/APNs → Phone notification
                                                                   ↓
                                                        Sound + Vibration + "Open App" button
```

## What Gets Built

### 1. Admin Page (`/admin`)

New page on the website with two buttons:
- **Send Water Alert** — pushes a water alarm notification to all registered phones
- **Send Upstream Alert** — pushes an upstream alarm notification to all registered phones

Location: `src/RiverSentry.UI.Shared/Pages/Admin.razor`
- Add to web nav rail (not visible in mobile app tabs)
- Eventually put behind authentication

### 2. API Endpoints

| Method | Endpoint | Purpose |
|--------|----------|---------|
| `POST` | `/api/notifications/test/water` | Send water alert push to all devices |
| `POST` | `/api/notifications/test/upstream` | Send upstream alert push to all devices |
| `POST` | `/api/notifications/register` | Mobile app registers its push token on startup |

New controller: `src/RiverSentry.Api/Controllers/NotificationsController.cs`

### 3. Azure Notification Hubs Dispatcher

Replace `StubNotificationDispatcher` with `AzureNotificationDispatcher`:

- File: `src/RiverSentry.Infrastructure/Services/AzureNotificationDispatcher.cs`
- NuGet: `Microsoft.Azure.NotificationHubs` (add to Infrastructure project)
- Implements existing `INotificationDispatcher` interface
- Sends push with alert type, device name, and "Open App" action
- Update DI registration in `DependencyInjection.cs`

### 4. Database — PushToken Entity

New entity to store device tokens:

```csharp
// src/RiverSentry.Domain/Entities/PushToken.cs
public class PushToken
{
    public Guid Id { get; set; }
    public string Token { get; set; }        // FCM or APNs token
    public string Platform { get; set; }      // "android" or "ios"
    public DateTime RegisteredAt { get; set; }
    public DateTime? LastUsedAt { get; set; }
}
```

- Add `DbSet<PushToken>` to `RiverSentryDbContext`
- Add EF migration
- Add `IPushTokenRepository` interface and implementation

### 5. Mobile App — Push Registration

**On startup (both platforms):**
- Register for push notifications with the OS
- Get device token (FCM token on Android, APNs token on iOS)
- Send token to API via `POST /api/notifications/register`

**Android (`Platforms/Android/`):**
- Add `google-services.json` from Firebase Console
- Add NuGet: `Xamarin.Firebase.Messaging`
- Create `RiverSentryFirebaseService : FirebaseMessagingService`
  - `OnNewToken()` → send token to API
  - `OnMessageReceived()` → trigger `AlarmNotificationService`

**iOS (`Platforms/iOS/`):**
- Already calls `RegisterForRemoteNotifications()` in `AppDelegate.cs`
- Add `RegisteredForRemoteNotifications` override to capture APNs token
- Send token to API
- Incoming push handled by existing `AlarmNotificationDelegate`

### 6. API Client Methods

Add to `IRiverSentryApiClient` and `RiverSentryApiClient`:

```csharp
Task SendTestWaterAlertAsync(CancellationToken ct = default);
Task SendTestUpstreamAlertAsync(CancellationToken ct = default);
Task RegisterPushTokenAsync(string token, string platform, CancellationToken ct = default);
```

## Azure Portal Setup

### 1. Create Notification Hub
- Azure Portal → Create Resource → "Notification Hub"
- Pricing: **Free** (500 active devices, 1M pushes/month)
- Note the **connection string** (DefaultFullSharedAccessSignature)

### 2. Configure FCM (Android)
- Go to [Firebase Console](https://console.firebase.google.com) → Create project
- Project Settings → Cloud Messaging → Server Key (or FCM v1 service account)
- Download `google-services.json` → place in `src/RiverSentry.Mobile/Platforms/Android/`
- In Notification Hub → Google (GCM/FCM) → paste Server Key

### 3. Configure APNs (iOS)
- Apple Developer Portal → Keys → create APNs key (.p8 file)
- In Notification Hub → Apple (APNS) → upload .p8 key
- Set to **Sandbox** for development, **Production** for App Store builds

### 4. API Configuration
Add to `src/RiverSentry.Api/appsettings.json`:
```json
{
  "Azure": {
    "NotificationHub": {
      "ConnectionString": "<DefaultFullSharedAccessSignature>",
      "HubName": "riversentry-notifications"
    }
  }
}
```

For production, store the connection string in Azure App Service → Configuration → App Settings.

## Push Notification Payload

### Android (FCM)
```json
{
  "notification": {
    "title": "⚠️ WATER ALARM",
    "body": "Device X has detected rising water levels! Seek higher ground immediately.",
    "channel_id": "flood_alerts",
    "sound": "flood_warning",
    "priority": "high"
  },
  "data": {
    "alert_type": "water",
    "device_name": "Device X",
    "action": "open_app"
  }
}
```

### iOS (APNs)
```json
{
  "aps": {
    "alert": {
      "title": "⚠️ WATER ALARM",
      "body": "Device X has detected rising water levels! Seek higher ground immediately."
    },
    "sound": "flood_warning.mp3",
    "interruption-level": "critical",
    "category": "FLOOD_ALERT"
  },
  "alert_type": "water",
  "device_name": "Device X"
}
```

## Notification Behavior

| Scenario | What Happens |
|----------|-------------|
| App is open | Notification appears, alarm sound + vibration plays immediately |
| App is in background | OS shows notification banner, alarm sound plays, tapping opens app |
| App is closed/killed | OS shows notification, alarm sound plays, tapping launches app |
| Phone is locked | Notification appears on lock screen with alarm sound + vibration |

### "Open App" Action Button
Both platforms support notification action buttons. The notification will include:
- **Default tap** → opens the app
- **"Open App" button** → same as tap (explicit action for clarity)
- **"Dismiss" action** → stops the alarm sound (already implemented in iOS `AlarmNotificationDelegate`)

## Existing Code That Connects

These files already have partial plumbing for push notifications:

| File | What's There |
|------|-------------|
| `INotificationDispatcher.cs` | Interface ready — just needs real implementation |
| `StubNotificationDispatcher.cs` | Stub to replace with Azure implementation |
| `DependencyInjection.cs` | DI registration to swap (`line 36`) |
| `AlertService.cs` | Already calls `_notificationDispatcher.SendAlertNotificationAsync()` (`line 69`) |
| `AlertHub.cs` | SignalR hub for real-time broadcast (complements push) |
| `AppDelegate.cs` (iOS) | Already calls `RegisterForRemoteNotifications()` |
| `AlarmNotificationService.cs` | Full alarm sound + vibration + notification display |
| `AlarmNotificationDelegate.cs` (iOS) | Handles notification tap and dismiss |
| `AndroidManifest.xml` | Already has internet permissions |
| `AlarmSoundPlayer.razor` | Has "Simulate" buttons (local only — admin page will trigger real push) |

## Implementation Order

1. **Azure Portal** — Create Notification Hub, configure FCM + APNs
2. **Database** — Add `PushToken` entity, migration
3. **API** — Registration endpoint + test alert endpoints
4. **Infrastructure** — `AzureNotificationDispatcher` (replace stub)
5. **Mobile Android** — Firebase setup, token registration, message handling
6. **Mobile iOS** — Token capture, send to API
7. **Website** — Admin page with test buttons
8. **Test end-to-end** — Send from admin → verify notification on phone

## Cost

| Component | Tier | Cost |
|-----------|------|------|
| Azure Notification Hub | Free | $0 (500 devices, 1M pushes/month) |
| Firebase (FCM) | Free | $0 (unlimited pushes) |
| Apple APNs | Free | $0 (included with Apple Developer account) |
