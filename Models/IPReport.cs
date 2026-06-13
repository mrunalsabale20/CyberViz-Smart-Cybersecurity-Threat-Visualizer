namespace SmartCyberViz.Models
{
    public class IPReport
    {
        public int Id { get; set; }
        public string IPAddress { get; set; } = string.Empty;
        public int AbuseConfidenceScore { get; set; }   // 0–100 from AbuseIPDB
        public string ISP { get; set; } = string.Empty;
        public string UsageType { get; set; } = string.Empty;
        public string CountryCode { get; set; } = string.Empty;
        public string Domain { get; set; } = string.Empty;
        public bool IsTor { get; set; } = false;
        public int TotalReports { get; set; }
        public DateTime LastReportedAt { get; set; }
        public DateTime CheckedAt { get; set; } = DateTime.UtcNow;
        public string RawResponse { get; set; } = string.Empty;  // JSON from API

        // Foreign key
        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }
    }
}