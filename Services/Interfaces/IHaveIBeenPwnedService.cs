namespace SmartCyberViz.Services.Interfaces
{
    public interface IHaveIBeenPwnedService
    {
        Task<HaveIBeenPwnedResult> CheckPasswordAsync(string password);
    }

    public class HaveIBeenPwnedResult
    {
        public bool IsPwned { get; set; }
        public int PwnedCount { get; set; }
        public string SHA1Prefix { get; set; } = string.Empty;
    }
}