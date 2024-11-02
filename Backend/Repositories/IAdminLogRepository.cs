using PhoneVault.Models;

namespace PhoneVault.Repositories
{
    public interface IAdminLogRepository
    {
        Task<IEnumerable<AdminLog>> GetAllAdminLogs();
        Task<AdminLog> GetAdminLogById(int id);
        Task AddLog(AdminLog log);
        Task DeleteLog(int id);
        Task UpdateLog(AdminLog log);
    }
}
