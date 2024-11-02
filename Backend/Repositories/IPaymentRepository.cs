using PhoneVault.Models;

namespace PhoneVault.Repositories
{
    public interface IPaymentRepository
    {
        Task<IEnumerable<Payment>> GetAllPayments();
        Task<Payment> GetPaymentById(int id);
        Task AddPayment(Payment payment);
        Task UpdatePayment(Payment payment);
        Task DeletePayment(int id);
    }

}
