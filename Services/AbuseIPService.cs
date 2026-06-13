using Newtonsoft.Json.Linq;
using SmartCyberViz.Services.Interfaces;

namespace SmartCyberViz.Services
{
    public class AbuseIPService : IAbuseIPService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public AbuseIPService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["ApiKeys:AbuseIPDB"] ?? string.Empty;
            _httpClient.BaseAddress = new Uri("https://api.abuseipdb.com/api/v2/");
            _httpClient.DefaultRequestHeaders.Add("Key", _apiKey);
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        public async Task<AbuseIPResult?> CheckIPAsync(string ipAddress)
        {
            try
            {
                var response = await _httpClient.GetAsync(
                    $"check?ipAddress={Uri.EscapeDataString(ipAddress)}&maxAgeInDays=90&verbose=true");

                if (!response.IsSuccessStatusCode)
                    return null;

                var json = await response.Content.ReadAsStringAsync();
                var data = JObject.Parse(json)?["data"];

                if (data == null) return null;

                return new AbuseIPResult
                {
                    IPAddress = data["ipAddress"]?.ToString() ?? ipAddress,
                    AbuseConfidenceScore = data["abuseConfidenceScore"]?.Value<int>() ?? 0,
                    CountryCode = data["countryCode"]?.ToString() ?? string.Empty,
                    ISP = data["isp"]?.ToString() ?? string.Empty,
                    Domain = data["domain"]?.ToString() ?? string.Empty,
                    UsageType = data["usageType"]?.ToString() ?? string.Empty,
                    IsTor = data["isTor"]?.Value<bool>() ?? false,
                    TotalReports = data["totalReports"]?.Value<int>() ?? 0,
                    LastReportedAt = data["lastReportedAt"] != null
                                        ? DateTime.Parse(data["lastReportedAt"]!.ToString())
                                        : DateTime.MinValue,
                    RawJson = json
                };
            }
            catch
            {
                return null;
            }
        }
    }
}