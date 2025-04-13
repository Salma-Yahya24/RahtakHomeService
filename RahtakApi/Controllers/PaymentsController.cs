using Interfaces;
using Microsoft.AspNetCore.Mvc;
using RahtakApi.Entities.Models;

namespace RahtakApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public PaymentsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // *********** GET: api/Payments ***********
        [HttpGet]
        public IActionResult GetPayments()
        {
            var payments = _unitOfWork.Payments.GetAll();
            return Ok(payments);
        }

        // *********** GET: api/Payments/5 ***********
        [HttpGet("{id}")]
        public IActionResult GetPayment(int id)
        {
            var payment = _unitOfWork.Payments.GetById(id);

            if (payment == null)
            {
                return NotFound();
            }

            return Ok(payment);
        }

        // *********** POST: api/Payments ***********
        [HttpPost]
        public IActionResult CreatePayment([FromBody] Payments payment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _unitOfWork.Payments.Add(payment);
            _unitOfWork.Save();

            return CreatedAtAction("GetPayment", new { id = payment.PaymentId }, payment);
        }

        // *********** PUT: api/Payments/5 ***********
        [HttpPut("{id}")]
        public IActionResult UpdatePayment(int id, [FromBody] Payments payment)
        {
            if (id != payment.PaymentId)
            {
                return BadRequest();
            }

            var existingPayment = _unitOfWork.Payments.GetById(id);
            if (existingPayment == null)
            {
                return NotFound();
            }

            existingPayment.BookingId = payment.BookingId;
            existingPayment.PaymentDate = payment.PaymentDate;
            existingPayment.Amount = payment.Amount;
            existingPayment.PaymentStatus = payment.PaymentStatus;
            existingPayment.PaymentMethodId = payment.PaymentMethodId;

            _unitOfWork.Payments.Update(existingPayment);
            _unitOfWork.Save();

            return NoContent();
        }

        // *********** DELETE: api/Payments/5 ***********
        [HttpDelete("{id}")]
        public IActionResult DeletePayment(int id)
        {
            var payment = _unitOfWork.Payments.GetById(id);
            if (payment == null)
            {
                return NotFound();
            }

            _unitOfWork.Payments.Delete(payment);
            _unitOfWork.Save();

            return NoContent();
        }
    }
}
