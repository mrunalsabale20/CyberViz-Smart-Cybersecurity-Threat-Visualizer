using Newtonsoft.Json.Linq;
using SmartCyberViz.Services.Interfaces;

namespace SmartCyberViz.Services
{
    public class IPStackService : IIPStackService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public IPStackService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["ApiKeys:IPStack"] ?? string.Empty;
            _httpClient.BaseAddress = new Uri("http://api.ipstack.com/");
        }

        public async Task<IPStackResult?> GetLocationAsync(string ipAddress)
        {
            try
            {
                var response = await _httpClient.GetAsync(
                    $"{Uri.EscapeDataString(ipAddress)}?access_key={_apiKey}&fields=ip,country_name,country_code,region_name,city,latitude,longitude,connection");

                if (!response.IsSuccessStatusCode) return null;

                var json = await response.Content.ReadAsStringAsync();
                var data = JObject.Parse(json);

                if (data == null) return null;

                return new IPStackResult
                {
                    IPAddress = data["ip"]?.ToString() ?? ipAddress,
                    CountryName = data["country_name"]?.ToString() ?? string.Empty,
                    CountryCode = data["country_code"]?.ToString() ?? string.Empty,
                    RegionName = data["region_name"]?.ToString() ?? string.Empty,
                    City = data["city"]?.ToString() ?? string.Empty,
                    Latitude = data["latitude"]?.Value<double>() ?? 0,
                    Longitude = data["longitude"]?.Value<double>() ?? 0,
                    ISP = data["connection"]?["isp"]?.ToString() ?? string.Empty
                };
            }
            catch
            {
                return null;
            }
        }
    }
}