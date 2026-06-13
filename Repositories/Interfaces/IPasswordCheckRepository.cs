using SmartCyberViz.Models;

namespace SmartCyberViz.Repositories.Interfaces
{
    public interface IPasswordCheckRepository
    {
        Task<IEnumerable<PasswordCheck>> GetAllAsync();
        Task<IEnumerable<PasswordCheck>> GetByUserIdAsync(string userId);
        Task<PasswordCheck?> GetByIdAsync(int id);
        Task<IEnumerable<PasswordCheck>> GetPwnedAsync();
        Task AddAsync(PasswordCheck passwordCheck);
        Task UpdateAsync(PasswordCheck passwordCheck);
        Task DeleteAsync(int id);
        Task<int> GetTotalCountAsync();
        Task<int> GetPwnedCountAsync();
    }
}