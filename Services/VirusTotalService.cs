using Newtonsoft.Json.Linq;
using SmartCyberViz.Services.Interfaces;
using System.Text;

namespace SmartCyberViz.Services
{
    public class VirusTotalService : IVirusTotalService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public VirusTotalService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["ApiKeys:VirusTotal"] ?? string.Empty;
            _httpClient.BaseAddress = new Uri("https://www.virustotal.com/api/v3/");
            _httpClient.DefaultRequestHeaders.Add("x-apikey", _apiKey);
        }

        public async Task<VirusTotalResult?> CheckUrlAsync(string url)
        {
            try
            {
                // Step 1 — Submit URL for analysis
                var formData = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("url", url)
                });

                var submitResponse = await _httpClient.PostAsync("urls", formData);
                if (!submitResponse.IsSuccessStatusCode) return null;

                var submitJson = await submitResponse.Content.ReadAsStringAsync();
                var analysisId = JObject.Parse(submitJson)?["data"]?["id"]?.ToString();
                if (string.IsNullOrEmpty(analysisId)) return null;

                // Step 2 — Poll analysis result (wait briefly for processing)
                await Task.Delay(3000);

                var resultResponse = await _httpClient.GetAsync($"analyses/{analysisId}");
                if (!resultResponse.IsSuccessStatusCode) return null;

                var resultJson = await resultResponse.Content.ReadAsStringAsync();
                var stats = JObject.Parse(resultJson)?["data"]?["attributes"]?["stats"];

                if (stats == null) return null;

                int malicious = stats["malicious"]?.Value<int>() ?? 0;
                int suspicious = stats["suspicious"]?.Value<int>() ?? 0;
                int harmless = stats["harmless"]?.Value<int>() ?? 0;
                int undetected = stats["undetected"]?.Value<int>() ?? 0;
                int total = malicious + suspicious + harmless + undetected;

                string verdict = malicious > 0 ? "Malicious"
                               : suspicious > 0 ? "Suspicious"
                               : "Clean";

                return new VirusTotalResult
                {
                    Url = url,
                    IsMalicious = malicious > 0,
                    MaliciousVendors = malicious,
                    TotalVendors = total,
                    Verdict = verdict,
                    Categories = string.Empty,
                    RawJson = resultJson
                };
            }
            catch
            {
                return null;
            }
        }
    }
}