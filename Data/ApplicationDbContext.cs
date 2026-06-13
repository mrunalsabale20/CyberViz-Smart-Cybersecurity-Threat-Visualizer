using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SmartCyberViz.Models;

namespace SmartCyberViz.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ThreatLog> ThreatLogs { get; set; }
        public DbSet<IPReport> IPReports { get; set; }
        public DbSet<PhishingCheck> PhishingChecks { get; set; }
        public DbSet<PasswordCheck> PasswordChecks { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // ThreatLog config
            builder.Entity<ThreatLog>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.IPAddress).IsRequired().HasMaxLength(45);
                entity.Property(e => e.ThreatType).HasMaxLength(100);
                entity.Property(e => e.Severity).HasMaxLength(20);
                entity.Property(e => e.Country).HasMaxLength(100);
                entity.Property(e => e.City).HasMaxLength(100);
                entity.HasOne(e => e.User)
                      .WithMany(u => u.ThreatLogs)
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            // IPReport config
            builder.Entity<IPReport>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.IPAddress).IsRequired().HasMaxLength(45);
                entity.Property(e => e.CountryCode).HasMaxLength(10);
                entity.Property(e => e.ISP).HasMaxLength(200);
                entity.HasOne(e => e.User)
                      .WithMany(u => u.IPReports)
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            // PhishingCheck config
            builder.Entity<PhishingCheck>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Url).IsRequired().HasMaxLength(2048);
                entity.Property(e => e.Verdict).HasMaxLength(50);
                entity.HasOne(e => e.User)
                      .WithMany(u => u.PhishingChecks)
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            // PasswordCheck config
            builder.Entity<PasswordCheck>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.PasswordHash).HasMaxLength(50);
                entity.Property(e => e.StrengthLabel).HasMaxLength(20);
                entity.HasOne(e => e.User)
                      .WithMany(u => u.PasswordChecks)
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.SetNull);
            });
        }
    }
}