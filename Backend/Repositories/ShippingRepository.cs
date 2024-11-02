using Microsoft.EntityFrameworkCore;
using PhoneVault.Data;
using PhoneVault.Models;
using PhoneVault.Repositories;
using System;

namespace PhoneVault.Repositories
{
    public class ShippingRepository: IShippingRepository
    {
        private readonly PhoneVaultContext _context;
        public ShippingRepository(PhoneVaultContext context)
        {
            _context = context;
        }

        public async Task AddShippingRecord(Shipping record)
        {
            if(record == null)
            {
                throw new ArgumentNullException(nameof(record));
            }
            await _context.Shipping.AddAsync(record);
            await _context.SaveChangesAsync();

        }

        public async Task DeleteShippingRecord(int id)
        {
            if (id == 0) 
            {
                throw new ArgumentNullException(nameof(id));
            }
            var record=await _context.Shipping.FindAsync(id);
            if (record == null) 
            {
                throw new ArgumentNullException(nameof(record));
            }
            _context.Shipping.Remove(record);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Shipping>> GetAllShippingRecords() =>
            await _context.Shipping.ToListAsync();

        public async Task<Shipping> GetShippingRecordById(int id) =>
            await _context.Shipping.FindAsync(id);

        public async Task UpdateShippingRecord(Shipping record)
        {
            if (record == null)
            {
                throw new ArgumentNullException(nameof(record));
            }
            _context.Entry(record).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
