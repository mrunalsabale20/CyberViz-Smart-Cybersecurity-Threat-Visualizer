using System.ComponentModel.DataAnnotations;

namespace SmartCyberViz.ViewModels
{
    public class PasswordViewModel
    {
        [Required(ErrorMessage = "Please enter a password")]
        [DataType(DataType.Password)]
        [Display(Name = "Password to Check")]
        public string Password { get; set; } = string.Empty;

        public bool HasResult { get; set; } = false;
        public bool IsPwned { get; set; }
        public int PwnedCount { get; set; }
        public int StrengthScore { get; set; }
        public string StrengthLabel { get; set; } = string.Empty;
        public string StrengthColor { get; set; } = string.Empty;
        public List<string> StrengthTips { get; set; } = new();
        public string ErrorMessage { get; set; } = string.Empty;
    }
}