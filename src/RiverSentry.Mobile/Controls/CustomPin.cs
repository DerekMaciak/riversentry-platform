using Microsoft.Maui.Controls.Maps;
using RiverSentry.Domain.Enums;

namespace RiverSentry.Mobile.Controls;

/// <summary>
/// Custom pin that supports custom images instead of default pin icons.
/// </summary>
public class CustomPin : Pin
{
    public static readonly BindableProperty ImageSourceProperty =
        BindableProperty.Create(
            nameof(ImageSource),
            typeof(ImageSource),
            typeof(CustomPin),
            default(ImageSource));

    public static readonly BindableProperty DeviceStateProperty =
        BindableProperty.Create(
            nameof(DeviceState),
            typeof(DeviceState),
            typeof(CustomPin),
            DeviceState.Offline);

    /// <summary>
    /// The image source for the custom pin icon.
    /// </summary>
    public ImageSource? ImageSource
    {
        get => (ImageSource?)GetValue(ImageSourceProperty);
        set => SetValue(ImageSourceProperty, value);
    }

    /// <summary>
    /// The device state for color coding.
    /// </summary>
    public DeviceState DeviceState
    {
        get => (DeviceState)GetValue(DeviceStateProperty);
        set => SetValue(DeviceStateProperty, value);
    }
}
