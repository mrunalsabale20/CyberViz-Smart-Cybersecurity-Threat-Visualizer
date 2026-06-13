using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartCyberViz.Repositories.Interfaces;

namespace SmartCyberViz.Controllers
{
    [Authorize]
    public class ThreatFeedController : Controller
    {
        private readonly IThreatLogRepository _threatLogRepo;
        private readonly IIPReportRepository _ipReportRepo;
        private readonly IPhishingCheckRepository _phishingRepo;
        private readonly IPasswordCheckRepository _passwordRepo;

        public ThreatFeedController(
            IThreatLogRepository threatLogRepo,
            IIPReportRepository ipReportRepo,
            IPhishingCheckRepository phishingRepo,
            IPasswordCheckRepository passwordRepo)
        {
            _threatLogRepo = threatLogRepo;
            _ipReportRepo = ipReportRepo;
            _phishingRepo = phishingRepo;
            _passwordRepo = passwordRepo;
        }

        public async Task<IActionResult> Index()
        {
            var vm = new ThreatFeedViewModel
            {
                RecentThreats = (await _threatLogRepo.GetRecentAsync(20)).ToList(),
                RecentIPReports = (await _ipReportRepo.GetAllAsync()).Take(10).ToList(),
                RecentPhishing = (await _phishingRepo.GetAllAsync()).Take(10).ToList(),
                HighRiskIPs = (await _ipReportRepo.GetHighRiskAsync(75)).Take(5).ToList(),
                MaliciousUrls = (await _phishingRepo.GetMaliciousAsync()).Take(5).ToList(),
                TotalThreats = await _threatLogRepo.GetTotalCountAsync(),
                TotalScans = await _phishingRepo.GetTotalCountAsync()
                                + await _ipReportRepo.GetTotalCountAsync()
                                + await _passwordRepo.GetTotalCountAsync()
            };

            return View(vm);
        }
    }

    // ── ViewModel defined here for simplicity ─────────────────────
    public class ThreatFeedViewModel
    {
        public List<SmartCyberViz.Models.ThreatLog> RecentThreats { get; set; } = new();
        public List<SmartCyberViz.Models.IPReport> RecentIPReports { get; set; } = new();
        public List<SmartCyberViz.Models.PhishingCheck> RecentPhishing { get; set; } = new();
        public List<SmartCyberViz.Models.IPReport> HighRiskIPs { get; set; } = new();
        public List<SmartCyberViz.Models.PhishingCheck> MaliciousUrls { get; set; } = new();
        public int TotalThreats { get; set; }
        public int TotalScans { get; set; }
    }
}