namespace SmartCyberViz.Services.Interfaces
{
    public interface IVirusTotalService
    {
        Task<VirusTotalResult?> CheckUrlAsync(string url);
    }

    public class VirusTotalResult
    {
        public string Url { get; set; } = string.Empty;
        public bool IsMalicious { get; set; }
        public int MaliciousVendors { get; set; }
        public int TotalVendors { get; set; }
        public string Verdict { get; set; } = string.Empty;
        public string Categories { get; set; } = string.Empty;
        public string RawJson { get; set; } = string.Empty;
    }
}