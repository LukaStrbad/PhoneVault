using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using PhoneVault.Models;
using PhoneVault.Repositories;
using PhoneVault.Services;

namespace PhoneVault.Services
{
    public class PaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        public PaymentService(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }
        public async Task <IEnumerable<Payment>> GetAllPayments() =>
            await _paymentRepository.GetAllPayments();
        public async Task<Payment> GetPaymentById(int id) =>
            await _paymentRepository.GetPaymentById(id);
        public async Task AddPayment(Payment payment)
        {
            if (!(payment == null))
            {
                await _paymentRepository.AddPayment(payment);
            }
            else
            {
                throw new ArgumentException(nameof(payment));
            }
        }
        public async Task UpdatePayment(Payment payment)
        {
            if (!(payment == null))
            {
                await _paymentRepository.UpdatePayment(payment);
            }
            else
            {
                throw new ArgumentException(nameof(payment));
            }
        }
        public async Task DeletePayment(int id)
        {
            if (!(id == null))
            {
                await _paymentRepository.DeletePayment(id);
            }
            else
            {
                throw new ArgumentException(nameof(id));
            }
        }
    }
}


