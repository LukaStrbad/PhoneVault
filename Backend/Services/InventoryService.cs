﻿using PhoneVault.Models;
using PhoneVault.Repositories;
using PhoneVault.Services;

namespace PhoneVault.Services
{
    public class InventoryService
    {
        private readonly IInventoryRepository _inventoryRepository;
        public InventoryService(IInventoryRepository inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }
        public async Task<IEnumerable<Inventory>> GetAllInventoryRecords() =>
            await _inventoryRepository.GetAllInventoryRecords();
        public async Task<Inventory> GetInventoryRecordById(int id) =>
            await _inventoryRepository.GetInventoryRecordById(id);
        public async Task AddInventoryRecord(Inventory record)
        {
            if(record == null) throw new ArgumentNullException(nameof(record));
            await _inventoryRepository.AddInventoryRecord(record);
        }
        public async Task UpdateInventoryRecord(Inventory record)
        {
            if( record == null) throw new ArgumentNullException( nameof(record));
            await _inventoryRepository.UpdateInventoryRecord(record);
        }
        public async Task DeleteInventoryRecord(int id)
        {
            if(id == null) throw new ArgumentNullException("id");
            await _inventoryRepository.DeleteInventoryRecord(id);
        }
     }
}
