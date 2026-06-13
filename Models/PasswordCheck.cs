namespace SmartCyberViz.Models
{
    public class PasswordCheck
    {
        public int Id { get; set; }
        public string PasswordHash { get; set; } = string.Empty;  // SHA-1 prefix only, never plain text
        public bool IsPwned { get; set; } = false;
        public int PwnedCount { get; set; }              // Times seen in breaches
        public int StrengthScore { get; set; }           // 0–100 calculated locally
        public string StrengthLabel { get; set; } = string.Empty; // Weak / Fair / Strong / Very Strong
        public DateTime CheckedAt { get; set; } = DateTime.UtcNow;

        // Foreign key
        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }
    }
}