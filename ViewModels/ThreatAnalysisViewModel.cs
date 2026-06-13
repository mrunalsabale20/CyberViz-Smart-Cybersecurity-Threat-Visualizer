using System.ComponentModel.DataAnnotations;

namespace SmartCyberViz.ViewModels
{
    public class ThreatAnalysisViewModel
    {
        [Required(ErrorMessage = "Please enter an IP address")]
        [Display(Name = "IP Address")]
        public string IPAddress { get; set; } = string.Empty;

        public bool HasResult { get; set; } = false;
        public int AbuseConfidenceScore { get; set; }
        public string CountryCode { get; set; } = string.Empty;
        public string ISP { get; set; } = string.Empty;
        public string Domain { get; set; } = string.Empty;
        public string UsageType { get; set; } = string.Empty;
        public bool IsTor { get; set; }
        public int TotalReports { get; set; }
        public DateTime LastReportedAt { get; set; }
        public string RiskLevel { get; set; } = string.Empty;
        public string RiskColor { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string CountryName { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
    }
}