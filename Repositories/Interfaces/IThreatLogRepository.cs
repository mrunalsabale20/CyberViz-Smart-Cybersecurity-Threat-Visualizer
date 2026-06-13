using SmartCyberViz.Models;

namespace SmartCyberViz.Repositories.Interfaces
{
    public interface IThreatLogRepository
    {
        Task<IEnumerable<ThreatLog>> GetAllAsync();
        Task<IEnumerable<ThreatLog>> GetByUserIdAsync(string userId);
        Task<ThreatLog?> GetByIdAsync(int id);
        Task<IEnumerable<ThreatLog>> GetRecentAsync(int count);
        Task<IEnumerable<ThreatLog>> GetBySeverityAsync(string severity);
        Task<Dictionary<string, int>> GetThreatCountByTypeAsync();
        Task<Dictionary<string, int>> GetThreatCountByCountryAsync();
        Task AddAsync(ThreatLog threatLog);
        Task UpdateAsync(ThreatLog threatLog);
        Task DeleteAsync(int id);
        Task<int> GetTotalCountAsync();
    }
}