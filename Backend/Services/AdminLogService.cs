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
        public async Task AddAdminLog(AdminLogDTO logDto)
        {
            if(logDto == null) throw new ArgumentNullException(nameof(logDto));
            var log = new AdminLog
            {
                AdminId = logDto.AdminId,
                Action = logDto.Action,
                Timestamp = logDto.Timestamp,
            };
            await _logRepository.AddLog(log);
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
