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
    public class PhishingController : Controller
    {
        private readonly IVirusTotalService _virusTotalService;
        private readonly IPhishingCheckRepository _phishingRepo;
        private readonly UserManager<ApplicationUser> _userManager;

        public PhishingController(
            IVirusTotalService virusTotalService,
            IPhishingCheckRepository phishingRepo,
            UserManager<ApplicationUser> userManager)
        {
            _virusTotalService = virusTotalService;
            _phishingRepo = phishingRepo;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new PhishingViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(PhishingViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await _virusTotalService.CheckUrlAsync(model.Url);

            if (result == null)
            {
                model.ErrorMessage = "Could not scan the URL. Please check your VirusTotal API key or try again.";
                return View(model);
            }

            // ── Populate ViewModel ────────────────────────────────
            model.HasResult = true;
            model.IsMalicious = result.IsMalicious;
            model.MaliciousVendors = result.MaliciousVendors;
            model.TotalVendors = result.TotalVendors;
            model.Verdict = result.Verdict;
            model.Categories = result.Categories;

            // ── Save to database ──────────────────────────────────
            var userId = _userManager.GetUserId(User);

            await _phishingRepo.AddAsync(new PhishingCheck
            {
                Url = model.Url,
                IsMalicious = result.IsMalicious,
                MaliciousVendors = result.MaliciousVendors,
                TotalVendors = result.TotalVendors,
                Verdict = result.Verdict,
                Categories = result.Categories,
                CheckedAt = DateTime.UtcNow,
                RawResponse = result.RawJson,
                UserId = userId
            });

            return View(model);
        }
    }
}