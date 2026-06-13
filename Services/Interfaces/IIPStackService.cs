namespace SmartCyberViz.Services.Interfaces
{
    public interface IIPStackService
    {
        Task<IPStackResult?> GetLocationAsync(string ipAddress);
    }

    public class IPStackResult
    {
        public string IPAddress { get; set; } = string.Empty;
        public string CountryName { get; set; } = string.Empty;
        public string CountryCode { get; set; } = string.Empty;
        public string RegionName { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string ISP { get; set; } = string.Empty;
    }
}