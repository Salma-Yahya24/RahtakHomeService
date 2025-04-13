using Interfaces;
using Microsoft.AspNetCore.Mvc;
using RahtakApi.Entities.Models;

namespace RahtakApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentMethodController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public PaymentMethodController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // *********** GET: api/PaymentMethods ***********
        [HttpGet]
        public IActionResult GetPaymentMethods()
        {
            var paymentMethods = _unitOfWork.PaymentMethods.GetAll();
            return Ok(paymentMethods);
        }

        // *********** GET: api/PaymentMethods/5 ***********
        [HttpGet("{id}")]
        public IActionResult GetPaymentMethod(int id)
        {
            var paymentMethod = _unitOfWork.PaymentMethods.GetById(id);

            if (paymentMethod == null)
            {
                return NotFound();
            }

            return Ok(paymentMethod);
        }

        // *********** POST: api/PaymentMethods ***********
        [HttpPost]
        public IActionResult CreatePaymentMethod([FromBody] PaymentMethod paymentMethod)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _unitOfWork.PaymentMethods.Add(paymentMethod);
            _unitOfWork.Save();

            return CreatedAtAction("GetPaymentMethod", new { id = paymentMethod.PaymentMethodId }, paymentMethod);
        }

        // *********** PUT: api/PaymentMethods/5 ***********
        [HttpPut("{id}")]
        public IActionResult UpdatePaymentMethod(int id, [FromBody] PaymentMethod paymentMethod)
        {
            if (id != paymentMethod.PaymentMethodId)
            {
                return BadRequest();
            }

            var existingPaymentMethod = _unitOfWork.PaymentMethods.GetById(id);
            if (existingPaymentMethod == null)
            {
                return NotFound();
            }

            existingPaymentMethod.PaymentTypeDesc = paymentMethod.PaymentTypeDesc;

            _unitOfWork.PaymentMethods.Update(existingPaymentMethod);
            _unitOfWork.Save();

            return NoContent();
        }

        // *********** DELETE: api/PaymentMethods/5 ***********
        [HttpDelete("{id}")]
        public IActionResult DeletePaymentMethod(int id)
        {
            var paymentMethod = _unitOfWork.PaymentMethods.GetById(id);
            if (paymentMethod == null)
            {
                return NotFound();
            }

            _unitOfWork.PaymentMethods.Delete(paymentMethod);
            _unitOfWork.Save();

            return NoContent();
        }
    }
}
