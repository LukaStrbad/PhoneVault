using PhoneVault.Models;

namespace PhoneVault.Repositories
{
    public interface IInventoryRepository
    {
        Task<IEnumerable<Inventory>> GetAllInventoryRecords();
        Task<Inventory> GetInventoryRecordById(int id);
        Task AddInventoryRecord(Inventory record);
        Task UpdateInventoryRecord(Inventory record);
        Task DeleteInventoryRecord(int id);

    }
}
