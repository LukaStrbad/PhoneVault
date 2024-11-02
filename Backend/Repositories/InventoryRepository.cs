
using Microsoft.EntityFrameworkCore;
using PhoneVault.Data;
using PhoneVault.Models;
using PhoneVault.Repositories;

namespace PhoneVault.Repositories
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly PhoneVaultContext _context;
        public InventoryRepository(PhoneVaultContext context) 
        {
            _context = context;
        }

        public async Task AddInventoryRecord(Inventory record)
        {
            if(record == null) throw new ArgumentNullException(nameof(record));
            await _context.Inventory.AddAsync(record);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteInventoryRecord(int id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }
            else
            {
                var record=await _context.Inventory.FindAsync(id);
                if(record == null)
                {
                    throw new ArgumentException(nameof(record));
                }
                else
                {
                    _context.Inventory.Remove(record);
                    await _context.SaveChangesAsync();
                }
            }
        }

        public async Task<IEnumerable<Inventory>> GetAllInventoryRecords() =>
            await _context.Inventory.ToListAsync();

        public async Task<Inventory> GetInventoryRecordById(int id) =>
            await _context.Inventory.FindAsync(id);

        public async Task UpdateInventoryRecord(Inventory record)
        {
            if( record == null) throw new ArgumentNullException( nameof(record));
            _context.Entry(record).State= EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
