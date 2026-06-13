using System.ComponentModel.DataAnnotations;

namespace SmartCyberViz.ViewModels
{
    public class PhishingViewModel
    {
        [Required(ErrorMessage = "Please enter a URL")]
        [Url(ErrorMessage = "Please enter a valid URL (include http:// or https://)")]
        [Display(Name = "URL to Check")]
        public string Url { get; set; } = string.Empty;

        public bool HasResult { get; set; } = false;
        public bool IsMalicious { get; set; }
        public int MaliciousVendors { get; set; }
        public int TotalVendors { get; set; }
        public string Verdict { get; set; } = string.Empty;
        public string Categories { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
    }
}