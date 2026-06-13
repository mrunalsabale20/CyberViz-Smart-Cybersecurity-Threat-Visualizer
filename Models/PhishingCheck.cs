namespace SmartCyberViz.Models
{
    public class PhishingCheck
    {
        public int Id { get; set; }
        public string Url { get; set; } = string.Empty;
        public bool IsMalicious { get; set; } = false;
        public int MaliciousVendors { get; set; }        // Count from VirusTotal
        public int TotalVendors { get; set; }
        public string Verdict { get; set; } = string.Empty;  // Clean / Suspicious / Malicious
        public string Categories { get; set; } = string.Empty;
        public DateTime CheckedAt { get; set; } = DateTime.UtcNow;
        public string RawResponse { get; set; } = string.Empty;

        // Foreign key
        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }
    }
}