using Microsoft.EntityFrameworkCore;
using SmartCyberViz.Data;
using SmartCyberViz.Models;
using SmartCyberViz.Repositories.Interfaces;

namespace SmartCyberViz.Repositories
{
    public class IPReportRepository : IIPReportRepository
    {
        private readonly ApplicationDbContext _context;

        public IPReportRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<IPReport>> GetAllAsync()
        {
            return await _context.IPReports
                .Include(r => r.User)
                .OrderByDescending(r => r.CheckedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<IPReport>> GetByUserIdAsync(string userId)
        {
            return await _context.IPReports
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.CheckedAt)
                .ToListAsync();
        }

        public async Task<IPReport?> GetByIdAsync(int id)
        {
            return await _context.IPReports
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IPReport?> GetByIPAddressAsync(string ipAddress)
        {
            return await _context.IPReports
                .FirstOrDefaultAsync(r => r.IPAddress == ipAddress);
        }

        public async Task<IEnumerable<IPReport>> GetHighRiskAsync(int minScore)
        {
            return await _context.IPReports
                .Where(r => r.AbuseConfidenceScore >= minScore)
                .OrderByDescending(r => r.AbuseConfidenceScore)
                .ToListAsync();
        }

        public async Task AddAsync(IPReport ipReport)
        {
            await _context.IPReports.AddAsync(ipReport);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(IPReport ipReport)
        {
            _context.IPReports.Update(ipReport);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var report = await _context.IPReports.FindAsync(id);
            if (report != null)
            {
                _context.IPReports.Remove(report);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> GetTotalCountAsync()
        {
            return await _context.IPReports.CountAsync();
        }
    }
}