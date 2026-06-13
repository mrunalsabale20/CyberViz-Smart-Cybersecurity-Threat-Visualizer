using Microsoft.AspNetCore.Identity;

namespace SmartCyberViz.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string Role { get; set; } = "User";

        // Navigation properties
        public ICollection<ThreatLog> ThreatLogs { get; set; } = new List<ThreatLog>();
        public ICollection<IPReport> IPReports { get; set; } = new List<IPReport>();
        public ICollection<PhishingCheck> PhishingChecks { get; set; } = new List<PhishingCheck>();
        public ICollection<PasswordCheck> PasswordChecks { get; set; } = new List<PasswordCheck>();
    }
}