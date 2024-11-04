using PhoneVault.Models;
using PhoneVault.Repositories;
using PhoneVault.Services;

namespace PhoneVault.Services
{
    public class ShippingService
    {
        private readonly IShippingRepository _shippingRepository;
        public ShippingService(IShippingRepository shippingRepository)
        {
            _shippingRepository = shippingRepository;
        }  
        public async Task<IEnumerable<Shipping>> GetAllShippingRecords() =>
            await _shippingRepository.GetAllShippingRecords();
        public async Task<Shipping> GetShippingRecordById(int id) =>
            await _shippingRepository.GetShippingRecordById(id);
        public async Task AddShippingRecord(Shipping shipping)
        {
            if(shipping == null) throw new ArgumentNullException(nameof(shipping));
            await _shippingRepository.AddShippingRecord(shipping);
        }
        public async Task UpdateShippingRecord(Shipping shipping)
        {
            if (shipping == null) throw new ArgumentNullException(nameof (shipping));
            await _shippingRepository.UpdateShippingRecord(shipping);
        }
        public async Task DeleteShippingRecord(int id)
        {
            if(id == 0) throw new ArgumentNullException("id");
            await _shippingRepository.DeleteShippingRecord(id);
        }
    }
}
