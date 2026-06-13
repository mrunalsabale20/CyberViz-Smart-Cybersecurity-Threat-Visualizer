using SmartCyberViz.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace SmartCyberViz.Services
{
    public class HaveIBeenPwnedService : IHaveIBeenPwnedService
    {
        private readonly HttpClient _httpClient;

        public HaveIBeenPwnedService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://api.pwnedpasswords.com/");
            // HIBP requires a user-agent header
            _httpClient.DefaultRequestHeaders.Add(
                "User-Agent", "SmartCyberViz-App");
        }

        public async Task<HaveIBeenPwnedResult> CheckPasswordAsync(string password)
        {
            try
            {
                // k-Anonymity model — only send first 5 chars of SHA-1
                // The plain-text password NEVER leaves the server
                var sha1 = ComputeSHA1(password);
                var prefix = sha1[..5];
                var suffix = sha1[5..].ToUpperInvariant();

                var response = await _httpClient.GetAsync($"range/{prefix}");
                if (!response.IsSuccessStatusCode)
                    return new HaveIBeenPwnedResult { IsPwned = false };

                var body = await response.Content.ReadAsStringAsync();

                // Response is a list of SUFFIX:COUNT pairs
                var lines = body.Split('\n');
                foreach (var line in lines)
                {
                    var parts = line.Trim().Split(':');
                    if (parts.Length == 2 && parts[0].Equals(suffix, StringComparison.OrdinalIgnoreCase))
                    {
                        int count = int.TryParse(parts[1], out var c) ? c : 0;
                        return new HaveIBeenPwnedResult
                        {
                            IsPwned = true,
                            PwnedCount = count,
                            SHA1Prefix = prefix
                        };
                    }
                }

                return new HaveIBeenPwnedResult
                {
                    IsPwned = false,
                    PwnedCount = 0,
                    SHA1Prefix = prefix
                };
            }
            catch
            {
                return new HaveIBeenPwnedResult { IsPwned = false };
            }
        }

        private static string ComputeSHA1(string input)
        {
            var bytes = SHA1.HashData(Encoding.UTF8.GetBytes(input));
            return Convert.ToHexString(bytes);
        }
    }
}