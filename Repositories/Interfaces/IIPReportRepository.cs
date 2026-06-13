using SmartCyberViz.Models;

namespace SmartCyberViz.Repositories.Interfaces
{
    public interface IIPReportRepository
    {
        Task<IEnumerable<IPReport>> GetAllAsync();
        Task<IEnumerable<IPReport>> GetByUserIdAsync(string userId);
        Task<IPReport?> GetByIdAsync(int id);
        Task<IPReport?> GetByIPAddressAsync(string ipAddress);
        Task<IEnumerable<IPReport>> GetHighRiskAsync(int minScore);
        Task AddAsync(IPReport ipReport);
        Task UpdateAsync(IPReport ipReport);
        Task DeleteAsync(int id);
        Task<int> GetTotalCountAsync();
    }
}