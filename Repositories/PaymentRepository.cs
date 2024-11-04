using Microsoft.EntityFrameworkCore;
using PhoneVault.Data;
using PhoneVault.Models;

namespace PhoneVault.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        public readonly PhoneVaultContext _context;
        public PaymentRepository(PhoneVaultContext context)
        {
            _context = context;
        }

        public async Task AddPayment(Payment payment)
        {
            if (!(payment == null))
            {
                await _context.Payments.AddAsync(payment);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentNullException(nameof(payment));
            }
        }

        public async Task DeletePayment(int id)
        {
            if(!(id==null))
            {
                var payment=await _context.Payments.FindAsync(id);
                if (!(payment == null))
                {
                    _context.Payments.Remove(payment);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new ArgumentNullException(nameof(payment));
                }
            }
            else
            {
                throw new ArgumentNullException(nameof(id));
            }
        }

        public async Task<IEnumerable<Payment>> GetAllPayments() =>
            await _context.Payments.ToListAsync();

        public async Task<Payment> GetPaymentById(int id) =>
            await _context.Payments.FindAsync(id);

        public async Task UpdatePayment(Payment payment)
        {
            if (!(payment == null))
            {
                _context.Entry(payment).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            else
            {
                {
                    throw new ArgumentNullException(nameof(payment));
                }
            }
        }
    }
}

