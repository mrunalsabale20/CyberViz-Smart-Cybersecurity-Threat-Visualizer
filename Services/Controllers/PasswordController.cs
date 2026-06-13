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
    public class PasswordController : Controller
    {
        private readonly IHaveIBeenPwnedService _hibpService;
        private readonly IPasswordCheckRepository _passwordRepo;
        private readonly UserManager<ApplicationUser> _userManager;

        public PasswordController(
            IHaveIBeenPwnedService hibpService,
            IPasswordCheckRepository passwordRepo,
            UserManager<ApplicationUser> userManager)
        {
            _hibpService = hibpService;
            _passwordRepo = passwordRepo;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new PasswordViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(PasswordViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            // ── Check breach via HIBP ─────────────────────────────
            var hibpResult = await _hibpService.CheckPasswordAsync(model.Password);

            // ── Calculate strength locally ────────────────────────
            var (score, label, color, tips) = CalculateStrength(model.Password);

            // ── Populate ViewModel ────────────────────────────────
            model.HasResult = true;
            model.IsPwned = hibpResult.IsPwned;
            model.PwnedCount = hibpResult.PwnedCount;
            model.StrengthScore = score;
            model.StrengthLabel = label;
            model.StrengthColor = color;
            model.StrengthTips = tips;

            // ── Save to database (hash prefix only, never plaintext) ──
            var userId = _userManager.GetUserId(User);

            await _passwordRepo.AddAsync(new PasswordCheck
            {
                PasswordHash = hibpResult.SHA1Prefix,
                IsPwned = hibpResult.IsPwned,
                PwnedCount = hibpResult.PwnedCount,
                StrengthScore = score,
                StrengthLabel = label,
                CheckedAt = DateTime.UtcNow,
                UserId = userId
            });

            // Clear password from model before returning view
            model.Password = string.Empty;

            return View(model);
        }

        // ── Local password strength calculator ────────────────────
        private static (int score, string label, string color, List<string> tips)
            CalculateStrength(string password)
        {
            int score = 0;
            var tips = new List<string>();

            if (password.Length >= 8) score += 20;
            else tips.Add("Use at least 8 characters");

            if (password.Length >= 12) score += 10;
            else tips.Add("Use 12+ characters for a stronger password");

            if (password.Any(char.IsUpper)) score += 20;
            else tips.Add("Add uppercase letters (A–Z)");

            if (password.Any(char.IsLower)) score += 20;
            else tips.Add("Add lowercase letters (a–z)");

            if (password.Any(char.IsDigit)) score += 15;
            else tips.Add("Add numbers (0–9)");

            if (password.Any(c => "!@#$%^&*()_+-=[]{}|;':\",./<>?".Contains(c)))
                score += 15;
            else tips.Add("Add special characters (!@#$%^&*)");

            score = Math.Min(score, 100);

            var (label, color) = score switch
            {
                >= 85 => ("Very Strong", "#00ff88"),
                >= 65 => ("Strong", "#00b4ff"),
                >= 40 => ("Fair", "#ffd700"),
                _ => ("Weak", "#ff3860")
            };

            return (score, label, color, tips);
        }
    }
}