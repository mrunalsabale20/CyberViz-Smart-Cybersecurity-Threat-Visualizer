using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SmartCyberViz.Models;
using SmartCyberViz.Repositories.Interfaces;
using SmartCyberViz.ViewModels;

namespace SmartCyberViz.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IThreatLogRepository _threatLogRepo;
        private readonly IIPReportRepository _ipReportRepo;
        private readonly IPhishingCheckRepository _phishingRepo;
        private readonly IPasswordCheckRepository _passwordRepo;
        private readonly UserManager<ApplicationUser> _userManager;

        public DashboardController(
            IThreatLogRepository threatLogRepo,
            IIPReportRepository ipReportRepo,
            IPhishingCheckRepository phishingRepo,
            IPasswordCheckRepository passwordRepo,
            UserManager<ApplicationUser> userManager)
        {
            _threatLogRepo = threatLogRepo;
            _ipReportRepo = ipReportRepo;
            _phishingRepo = phishingRepo;
            _passwordRepo = passwordRepo;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var recentThreats = await _threatLogRepo.GetRecentAsync(10);

            var vm = new DashboardViewModel
            {
                TotalThreats = await _threatLogRepo.GetTotalCountAsync(),
                TotalIPReports = await _ipReportRepo.GetTotalCountAsync(),
                TotalPhishingChecks = await _phishingRepo.GetTotalCountAsync(),
                TotalPasswordChecks = await _passwordRepo.GetTotalCountAsync(),
                MaliciousUrls = await _phishingRepo.GetMaliciousCountAsync(),
                PwnedPasswords = await _passwordRepo.GetPwnedCountAsync(),
                ThreatsByType = await _threatLogRepo.GetThreatCountByTypeAsync(),
                ThreatsByCountry = await _threatLogRepo.GetThreatCountByCountryAsync(),
                RecentThreats = recentThreats.Select(t => new RecentThreatItem
                {
                    IPAddress = t.IPAddress,
                    ThreatType = t.ThreatType,
                    Severity = t.Severity,
                    Country = t.Country,
                    DetectedAt = t.DetectedAt
                }).ToList()
            };

            return View(vm);
        }
    }
}