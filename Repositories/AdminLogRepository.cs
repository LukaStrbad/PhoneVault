using Microsoft.EntityFrameworkCore;
using PhoneVault.Data;
using PhoneVault.Models;
using PhoneVault.Repositories;

namespace PhoneVault.Repositories
{
    public class AdminLogRepository: IAdminLogRepository
    {
        private readonly PhoneVaultContext _context;
        public AdminLogRepository(PhoneVaultContext context)
        {
            _context = context;
        }

        public async Task AddLog(AdminLog log)
        {
            await _context.AdminLogs.AddAsync(log);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteLog(int id)
        {
            var log=await _context.AdminLogs.FindAsync(id);
            if (!(log == null))
            {
                _context.AdminLogs.Remove(log);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<AdminLog> GetAdminLogById(int id) =>
                    await _context.AdminLogs.FindAsync(id);

        public async Task<IEnumerable<AdminLog>> GetAllAdminLogs() =>
            await _context.AdminLogs.ToListAsync();

        public async Task UpdateLog(AdminLog log)
        {
            _context.Entry(log).State= EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
