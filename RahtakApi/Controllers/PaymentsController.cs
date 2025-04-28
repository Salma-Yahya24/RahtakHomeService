using Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RahtakApi.Entities.DTOs;
using RahtakApi.Entities.Models;
using System.Security.Claims;

namespace RahtakApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // التوكن مطلوب لجميع العمليات في هذا الـ Controller
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
        public IActionResult CreatePayment([FromBody] PaymentDetails paymentDetails)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // استخدام التوكن لاستخراج معلومات المستخدم
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized(new { message = "User not authenticated." });
            }

            int userId = int.Parse(userIdClaim.Value);

            var booking = _unitOfWork.Bookings.GetById(paymentDetails.BookingId);
            if (booking == null)
            {
                return NotFound(new { message = "Booking not found." });
            }

            // إنشاء عنوان جديد وربطه بالـ User
            var address = new Address
            {
                UserId = userId,
                Street = paymentDetails.Address.Street,
                City = paymentDetails.Address.City,
                State = paymentDetails.Address.State,
                ZipCode = paymentDetails.Address.ZipCode,
                Country = paymentDetails.Address.Country
            };

            try
            {
                _unitOfWork.Addresses.Add(address);
                _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while saving the address.", error = ex.Message });
            }

            // إنشاء الدفع وربطه بالحجز
            var payment = new Payments
            {
                BookingId = paymentDetails.BookingId,
                PaymentDate = DateTime.UtcNow,
                Amount = paymentDetails.Amount,
                PaymentStatus = "Pending", // ستتغير هذه الحالة بناءً على طريقة الدفع
                PaymentMethodId = paymentDetails.PaymentMethodId
            };

            try
            {
                _unitOfWork.Payments.Add(payment);
                _unitOfWork.Save();

                // تحديث حالة الدفع بناءً على طريقة الدفع
                if (paymentDetails.PaymentMethodId == 1) // مثال: 1 يعني دفع كاش
                {
                    payment.PaymentStatus = "Completed";
                }
                else
                {
                    // لو كان الدفع أونلاين (مثال: Stripe)
                    payment.PaymentStatus = "Processing";
                }

                _unitOfWork.Payments.Update(payment);
                _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing the payment.", error = ex.Message });
            }

            return CreatedAtAction("GetPayment", new { id = payment.PaymentId }, payment);
        }

        // *********** PUT: api/Payments/5 ***********
        [HttpPut("{id}")]
        public IActionResult UpdatePayment(int id, [FromBody] PaymentDetails paymentDetails)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingPayment = _unitOfWork.Payments.GetById(id);
            if (existingPayment == null)
            {
                return NotFound(new { message = "Payment not found." });
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized(new { message = "User not authenticated." });
            }

            int userId = int.Parse(userIdClaim.Value);

            // تحديث بيانات الدفع
            existingPayment.Amount = paymentDetails.Amount;
            existingPayment.PaymentMethodId = paymentDetails.PaymentMethodId;
            existingPayment.PaymentDate = DateTime.UtcNow;

            // تحديث حالة الدفع بناءً على طريقة الدفع
            if (paymentDetails.PaymentMethodId == 1) // كاش
            {
                existingPayment.PaymentStatus = "Completed";
            }
            else
            {
                existingPayment.PaymentStatus = "Processing";
            }

            try
            {
                _unitOfWork.Payments.Update(existingPayment);
                _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the payment.", error = ex.Message });
            }

            // ممكن كمان تحدث العنوان لو حابب (اختياري)
            var existingAddress = _unitOfWork.Addresses.FindAll(a => a.UserId == userId).FirstOrDefault();
            if (existingAddress != null)
            {
                existingAddress.Street = paymentDetails.Address.Street;
                existingAddress.City = paymentDetails.Address.City;
                existingAddress.State = paymentDetails.Address.State;
                existingAddress.ZipCode = paymentDetails.Address.ZipCode;
                existingAddress.Country = paymentDetails.Address.Country;

                try
                {
                    _unitOfWork.Addresses.Update(existingAddress);
                    _unitOfWork.Save();
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new { message = "An error occurred while updating the address.", error = ex.Message });
                }
            }

            return Ok(existingPayment);
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
