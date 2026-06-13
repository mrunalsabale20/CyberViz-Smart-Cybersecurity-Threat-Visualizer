using Microsoft.EntityFrameworkCore;
using SmartCyberViz.Data;
using SmartCyberViz.Models;
using SmartCyberViz.Repositories.Interfaces;

namespace SmartCyberViz.Repositories
{
    public class PasswordCheckRepository : IPasswordCheckRepository
    {
        private readonly ApplicationDbContext _context;

        public PasswordCheckRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PasswordCheck>> GetAllAsync()
        {
            return await _context.PasswordChecks
                .Include(p => p.User)
                .OrderByDescending(p => p.CheckedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<PasswordCheck>> GetByUserIdAsync(string userId)
        {
            return await _context.PasswordChecks
                .Where(p => p.UserId == userId)
                .OrderByDescending(p => p.CheckedAt)
                .ToListAsync();
        }

        public async Task<PasswordCheck?> GetByIdAsync(int id)
        {
            return await _context.PasswordChecks
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<PasswordCheck>> GetPwnedAsync()
        {
            return await _context.PasswordChecks
                .Where(p => p.IsPwned)
                .OrderByDescending(p => p.CheckedAt)
                .ToListAsync();
        }

        public async Task AddAsync(PasswordCheck passwordCheck)
        {
            await _context.PasswordChecks.AddAsync(passwordCheck);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(PasswordCheck passwordCheck)
        {
            _context.PasswordChecks.Update(passwordCheck);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var check = await _context.PasswordChecks.FindAsync(id);
            if (check != null)
            {
                _context.PasswordChecks.Remove(check);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> GetTotalCountAsync()
        {
            return await _context.PasswordChecks.CountAsync();
        }

        public async Task<int> GetPwnedCountAsync()
        {
            return await _context.PasswordChecks.CountAsync(p => p.IsPwned);
        }
    }
}