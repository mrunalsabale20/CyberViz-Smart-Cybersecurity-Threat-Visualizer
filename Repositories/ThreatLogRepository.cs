using Microsoft.EntityFrameworkCore;
using SmartCyberViz.Data;
using SmartCyberViz.Models;
using SmartCyberViz.Repositories.Interfaces;

namespace SmartCyberViz.Repositories
{
    public class ThreatLogRepository : IThreatLogRepository
    {
        private readonly ApplicationDbContext _context;

        public ThreatLogRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ThreatLog>> GetAllAsync()
        {
            return await _context.ThreatLogs
                .Include(t => t.User)
                .OrderByDescending(t => t.DetectedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<ThreatLog>> GetByUserIdAsync(string userId)
        {
            return await _context.ThreatLogs
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.DetectedAt)
                .ToListAsync();
        }

        public async Task<ThreatLog?> GetByIdAsync(int id)
        {
            return await _context.ThreatLogs
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<ThreatLog>> GetRecentAsync(int count)
        {
            return await _context.ThreatLogs
                .OrderByDescending(t => t.DetectedAt)
                .Take(count)
                .ToListAsync();
        }

        public async Task<IEnumerable<ThreatLog>> GetBySeverityAsync(string severity)
        {
            return await _context.ThreatLogs
                .Where(t => t.Severity == severity)
                .OrderByDescending(t => t.DetectedAt)
                .ToListAsync();
        }

        public async Task<Dictionary<string, int>> GetThreatCountByTypeAsync()
        {
            return await _context.ThreatLogs
                .GroupBy(t => t.ThreatType)
                .Select(g => new { Type = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Type, x => x.Count);
        }

        public async Task<Dictionary<string, int>> GetThreatCountByCountryAsync()
        {
            return await _context.ThreatLogs
                .GroupBy(t => t.Country)
                .Select(g => new { Country = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Country, x => x.Count);
        }

        public async Task AddAsync(ThreatLog threatLog)
        {
            await _context.ThreatLogs.AddAsync(threatLog);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ThreatLog threatLog)
        {
            _context.ThreatLogs.Update(threatLog);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var threatLog = await _context.ThreatLogs.FindAsync(id);
            if (threatLog != null)
            {
                _context.ThreatLogs.Remove(threatLog);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> GetTotalCountAsync()
        {
            return await _context.ThreatLogs.CountAsync();
        }
    }
}