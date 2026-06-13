namespace SmartCyberViz.Models
{
    public class ThreatLog
    {
        public int Id { get; set; }
        public string IPAddress { get; set; } = string.Empty;
        public string ThreatType { get; set; } = string.Empty;
        public string Severity { get; set; } = string.Empty;   // Low / Medium / High / Critical
        public string Country { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime DetectedAt { get; set; } = DateTime.UtcNow;
        public bool IsResolved { get; set; } = false;

        // Foreign key
        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }
    }
}