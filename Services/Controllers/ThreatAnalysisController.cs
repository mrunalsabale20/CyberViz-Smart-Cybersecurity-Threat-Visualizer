using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SmartCyberViz.Models;
using SmartCyberViz.Repositories.Interfaces;
using SmartCyberViz.Services.Interfaces;
using SmartCyberViz.ViewModels;

namespace SmartCyberViz.Controllers
{
    [Authorize]
    public class ThreatAnalysisController : Controller
    {
        private readonly IAbuseIPService _abuseIPService;
        private readonly IIPStackService _ipStackService;
        private readonly IIPReportRepository _ipReportRepo;
        private readonly UserManager<ApplicationUser> _userManager;

        public ThreatAnalysisController(
            IAbuseIPService abuseIPService,
            IIPStackService ipStackService,
            IIPReportRepository ipReportRepo,
            UserManager<ApplicationUser> userManager)
        {
            _abuseIPService = abuseIPService;
            _ipStackService = ipStackService;
            _ipReportRepo = ipReportRepo;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new ThreatAnalysisViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ThreatAnalysisViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            // ── Call AbuseIPDB ────────────────────────────────────
            var abuseResult = await _abuseIPService.CheckIPAsync(model.IPAddress);

            if (abuseResult == null)
            {
                model.ErrorMessage = "Could not retrieve data. Check your AbuseIPDB API key or the IP address entered.";
                return View(model);
            }

            // ── Call IPStack for geo-location ─────────────────────
            var locationResult = await _ipStackService.GetLocationAsync(model.IPAddress);

            // ── Determine risk level ──────────────────────────────
            var (riskLevel, riskColor) = abuseResult.AbuseConfidenceScore switch
            {
                >= 80 => ("Critical", "#ff3860"),
                >= 50 => ("High", "#ff6b35"),
                >= 20 => ("Medium", "#ffd700"),
                _ => ("Low", "#00b4ff")
            };

            // ── Populate ViewModel ────────────────────────────────
            model.HasResult = true;
            model.AbuseConfidenceScore = abuseResult.AbuseConfidenceScore;
            model.CountryCode = abuseResult.CountryCode;
            model.ISP = abuseResult.ISP;
            model.Domain = abuseResult.Domain;
            model.UsageType = abuseResult.UsageType;
            model.IsTor = abuseResult.IsTor;
            model.TotalReports = abuseResult.TotalReports;
            model.LastReportedAt = abuseResult.LastReportedAt;
            model.RiskLevel = riskLevel;
            model.RiskColor = riskColor;
            model.City = locationResult?.City ?? string.Empty;
            model.CountryName = locationResult?.CountryName ?? string.Empty;
            model.Latitude = locationResult?.Latitude ?? 0;
            model.Longitude = locationResult?.Longitude ?? 0;

            // ── Save to database ──────────────────────────────────
            var userId = _userManager.GetUserId(User);

            var report = new IPReport
            {
                IPAddress = model.IPAddress,
                AbuseConfidenceScore = abuseResult.AbuseConfidenceScore,
                ISP = abuseResult.ISP,
                UsageType = abuseResult.UsageType,
                CountryCode = abuseResult.CountryCode,
                Domain = abuseResult.Domain,
                IsTor = abuseResult.IsTor,
                TotalReports = abuseResult.TotalReports,
                LastReportedAt = abuseResult.LastReportedAt,
                CheckedAt = DateTime.UtcNow,
                RawResponse = abuseResult.RawJson,
                UserId = userId
            };

            await _ipReportRepo.AddAsync(report);

            return View(model);
        }
    }
}