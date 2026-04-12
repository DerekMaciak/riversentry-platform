namespace RiverSentry.Contracts.DTOs;

/// <summary>
/// Dashboard statistics summary.
/// </summary>
public class DashboardStats
{
    public int TotalDevices { get; set; }
    public int OnlineDevices { get; set; }
    public int OfflineDevices { get; set; }
    public int TotalFamilies { get; set; }
    public int ActiveAlerts { get; set; }
}
