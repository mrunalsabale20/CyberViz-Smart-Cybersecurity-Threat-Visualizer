using Microsoft.EntityFrameworkCore;
using SmartCyberViz.Data;
using SmartCyberViz.Models;
using SmartCyberViz.Repositories.Interfaces;

namespace SmartCyberViz.Repositories
{
    public class PhishingCheckRepository : IPhishingCheckRepository
    {
        private readonly ApplicationDbContext _context;

        public PhishingCheckRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PhishingCheck>> GetAllAsync()
        {
            return await _context.PhishingChecks
                .Include(p => p.User)
                .OrderByDescending(p => p.CheckedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<PhishingCheck>> GetByUserIdAsync(string userId)
        {
            return await _context.PhishingChecks
                .Where(p => p.UserId == userId)
                .OrderByDescending(p => p.CheckedAt)
                .ToListAsync();
        }

        public async Task<PhishingCheck?> GetByIdAsync(int id)
        {
            return await _context.PhishingChecks
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<PhishingCheck?> GetByUrlAsync(string url)
        {
            return await _context.PhishingChecks
                .FirstOrDefaultAsync(p => p.Url == url);
        }

        public async Task<IEnumerable<PhishingCheck>> GetMaliciousAsync()
        {
            return await _context.PhishingChecks
                .Where(p => p.IsMalicious)
                .OrderByDescending(p => p.CheckedAt)
                .ToListAsync();
        }

        public async Task AddAsync(PhishingCheck phishingCheck)
        {
            await _context.PhishingChecks.AddAsync(phishingCheck);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(PhishingCheck phishingCheck)
        {
            _context.PhishingChecks.Update(phishingCheck);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var check = await _context.PhishingChecks.FindAsync(id);
            if (check != null)
            {
                _context.PhishingChecks.Remove(check);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> GetTotalCountAsync()
        {
            return await _context.PhishingChecks.CountAsync();
        }

        public async Task<int> GetMaliciousCountAsync()
        {
            return await _context.PhishingChecks.CountAsync(p => p.IsMalicious);
        }
    }
}