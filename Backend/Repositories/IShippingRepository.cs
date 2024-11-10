using PhoneVault.Models;

namespace PhoneVault.Repositories
{
    public interface IShippingRepository
    {
        Task<IEnumerable<Shipping>> GetAllShippingRecords();
        Task<Shipping> GetShippingRecordById(int id);
        Task AddShippingRecord(Shipping record);
        Task UpdateShippingRecord(Shipping record);
        Task DeleteShippingRecord(int id);
    }

}
