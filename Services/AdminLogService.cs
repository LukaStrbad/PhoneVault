using PhoneVault.Models;
using PhoneVault.Repositories;

namespace PhoneVault.Services
{
    public class AdminLogService
    {
        private readonly IAdminLogRepository _logRepository;
        public AdminLogService(IAdminLogRepository logRepository)
        {
            _logRepository = logRepository;
        }
        public async Task<IEnumerable<AdminLog>> GetAllAdminLogs() =>
            await _logRepository.GetAllAdminLogs();
        public async Task<AdminLog> GetAdminLogById(int id) =>
            await _logRepository.GetAdminLogById(id);
        public async Task AddAdminLog(AdminLog log)
        {
            if (!(log == null))
            {
                await _logRepository.AddLog(log);
            }
            else
            {
                throw new ArgumentNullException(nameof(log));
            }
        }
        public async Task UpdateAdminLog(AdminLog log)
        {
            if(!(log == null))
            {
                await _logRepository.UpdateLog(log);
            }
            else
            {
                throw new ArgumentNullException(nameof(log));
            }
        }
        public async Task DeleteAdminLog(int id)
        {
            if(!(id== null))
            {
                await _logRepository.DeleteLog(id);
            }
            else
            {
                throw new ArgumentNullException(nameof(id));
            }
        }

    }
}
