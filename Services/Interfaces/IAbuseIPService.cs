namespace SmartCyberViz.Services.Interfaces
{
    public interface IAbuseIPService
    {
        Task<AbuseIPResult?> CheckIPAsync(string ipAddress);
    }

    public class AbuseIPResult
    {
        public string IPAddress { get; set; } = string.Empty;
        public int AbuseConfidenceScore { get; set; }
        public string CountryCode { get; set; } = string.Empty;
        public string ISP { get; set; } = string.Empty;
        public string Domain { get; set; } = string.Empty;
        public string UsageType { get; set; } = string.Empty;
        public bool IsTor { get; set; }
        public int TotalReports { get; set; }
        public DateTime LastReportedAt { get; set; }
        public string RawJson { get; set; } = string.Empty;
    }
}