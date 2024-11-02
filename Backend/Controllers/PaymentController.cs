using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using PhoneVault.Models;
using PhoneVault.Services;

namespace PhoneVault.Controllers
{
    [Route("api/payment")]
    [ApiController]
    public class PaymentController: ControllerBase
    {
        private readonly PaymentService _paymentService;
        public PaymentController(PaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Payment>>> GetAllPayments()
        {
            var payments = await _paymentService.GetAllPayments();
            if (!(payments == null))
            {
                return Ok(payments);
            }
            else
            {
                return NotFound();
            }
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Payment>> GetPaymentById(int id)
        {
            var payment = await _paymentService.GetPaymentById(id);
            if (!(payment == null))
            {
                return Ok(payment);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<ActionResult> AddPayment(Payment payment)
        {
            if (!(payment == null))
            {
                await _paymentService.AddPayment(payment);
                return Ok(payment);
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpPut]
        public async Task<ActionResult> UpdatePayment(Payment payment)
        {
            if (payment == null)
            {
                return BadRequest();
            }
            await _paymentService.UpdatePayment(payment);
            return Ok(payment);
        }

        [HttpDelete]
        public async Task<ActionResult> DeletePayment(int id)
        {
            if(id == null)
            {
                return BadRequest();
            }
            else
            {
                await _paymentService.DeletePayment(id);
                return Ok();
            }
        }
    }
}







