using SmartCyberViz.Models;

namespace SmartCyberViz.Repositories.Interfaces
{
    public interface IPhishingCheckRepository
    {
        Task<IEnumerable<PhishingCheck>> GetAllAsync();
        Task<IEnumerable<PhishingCheck>> GetByUserIdAsync(string userId);
        Task<PhishingCheck?> GetByIdAsync(int id);
        Task<PhishingCheck?> GetByUrlAsync(string url);
        Task<IEnumerable<PhishingCheck>> GetMaliciousAsync();
        Task AddAsync(PhishingCheck phishingCheck);
        Task UpdateAsync(PhishingCheck phishingCheck);
        Task DeleteAsync(int id);
        Task<int> GetTotalCountAsync();
        Task<int> GetMaliciousCountAsync();
    }
}