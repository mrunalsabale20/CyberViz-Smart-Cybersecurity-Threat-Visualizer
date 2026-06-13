using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartCyberViz.Repositories.Interfaces;

namespace SmartCyberViz.Controllers
{
    [Authorize]
    public class ThreatMapController : Controller
    {
        private readonly IThreatLogRepository _threatLogRepo;

        public ThreatMapController(IThreatLogRepository threatLogRepo)
        {
            _threatLogRepo = threatLogRepo;
        }

        public async Task<IActionResult> Index()
        {
            var threats = await _threatLogRepo.GetAllAsync();
            return View(threats);
        }

        // Returns JSON for the map markers
        [HttpGet]
        public async Task<IActionResult> GetMapData()
        {
            var threats = await _threatLogRepo.GetAllAsync();

            var mapData = threats
                .Where(t => t.Latitude != 0 && t.Longitude != 0)
                .Select(t => new
                {
                    ip = t.IPAddress,
                    type = t.ThreatType,
                    severity = t.Severity,
                    country = t.Country,
                    city = t.City,
                    lat = t.Latitude,
                    lng = t.Longitude,
                    detected = t.DetectedAt.ToString("dd MMM yyyy HH:mm")
                });

            return Json(mapData);
        }

        // Seed some demo data so the map is not empty
        [HttpPost]
        public async Task<IActionResult> SeedDemoData()
        {
            var demo = new List<SmartCyberViz.Models.ThreatLog>
            {
                new() { IPAddress="103.21.244.0",  ThreatType="DDoS",          Severity="Critical", Country="China",         City="Beijing",      Latitude=39.9042,  Longitude=116.4074, DetectedAt=DateTime.UtcNow.AddMinutes(-5)  },
                new() { IPAddress="185.220.101.5", ThreatType="Port Scan",     Severity="High",     Country="Russia",        City="Moscow",       Latitude=55.7558,  Longitude=37.6173,  DetectedAt=DateTime.UtcNow.AddMinutes(-12) },
                new() { IPAddress="45.33.32.156",  ThreatType="Brute Force",   Severity="High",     Country="USA",           City="New York",     Latitude=40.7128,  Longitude=-74.0060, DetectedAt=DateTime.UtcNow.AddMinutes(-20) },
                new() { IPAddress="91.108.4.0",    ThreatType="Phishing",      Severity="Medium",   Country="Germany",       City="Berlin",       Latitude=52.5200,  Longitude=13.4050,  DetectedAt=DateTime.UtcNow.AddMinutes(-35) },
                new() { IPAddress="198.54.117.10", ThreatType="Malware",       Severity="Critical", Country="Brazil",        City="São Paulo",    Latitude=-23.5505, Longitude=-46.6333, DetectedAt=DateTime.UtcNow.AddMinutes(-50) },
                new() { IPAddress="5.188.206.14",  ThreatType="SQL Injection", Severity="High",     Country="Ukraine",       City="Kyiv",         Latitude=50.4501,  Longitude=30.5234,  DetectedAt=DateTime.UtcNow.AddMinutes(-60) },
                new() { IPAddress="80.82.77.33",   ThreatType="XSS Attack",    Severity="Medium",   Country="Netherlands",   City="Amsterdam",    Latitude=52.3676,  Longitude=4.9041,   DetectedAt=DateTime.UtcNow.AddMinutes(-75) },
                new() { IPAddress="119.29.29.29",  ThreatType="DNS Spoofing",  Severity="Medium",   Country="India",         City="Mumbai",       Latitude=19.0760,  Longitude=72.8777,  DetectedAt=DateTime.UtcNow.AddMinutes(-90) },
                new() { IPAddress="41.223.40.0",   ThreatType="Ransomware",    Severity="Critical", Country="Nigeria",       City="Lagos",        Latitude=6.5244,   Longitude=3.3792,   DetectedAt=DateTime.UtcNow.AddMinutes(-100)},
                new() { IPAddress="202.12.29.0",   ThreatType="Data Exfil",   Severity="High",     Country="Japan",         City="Tokyo",        Latitude=35.6762,  Longitude=139.6503, DetectedAt=DateTime.UtcNow.AddMinutes(-110)},
                new() { IPAddress="177.54.144.0",  ThreatType="Botnet",        Severity="Medium",   Country="Argentina",     City="Buenos Aires", Latitude=-34.6037, Longitude=-58.3816, DetectedAt=DateTime.UtcNow.AddMinutes(-120)},
                new() { IPAddress="196.202.120.0", ThreatType="Brute Force",   Severity="Low",      Country="South Africa",  City="Cape Town",    Latitude=-33.9249, Longitude=18.4241,  DetectedAt=DateTime.UtcNow.AddMinutes(-130)},
            };

            foreach (var t in demo)
                await _threatLogRepo.AddAsync(t);

            return Json(new { success = true, count = demo.Count });
        }
    }
}