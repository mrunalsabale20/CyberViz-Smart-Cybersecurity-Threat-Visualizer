namespace SmartCyberViz.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalThreats { get; set; }
        public int TotalIPReports { get; set; }
        public int TotalPhishingChecks { get; set; }
        public int TotalPasswordChecks { get; set; }
        public int MaliciousUrls { get; set; }
        public int PwnedPasswords { get; set; }
        public Dictionary<string, int> ThreatsByType { get; set; } = new();
        public Dictionary<string, int> ThreatsByCountry { get; set; } = new();
        public List<RecentThreatItem> RecentThreats { get; set; } = new();
    }

    public class RecentThreatItem
    {
        public string IPAddress { get; set; } = string.Empty;
        public string ThreatType { get; set; } = string.Empty;
        public string Severity { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public DateTime DetectedAt { get; set; }
    }
}