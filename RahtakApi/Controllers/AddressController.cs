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
    [Authorize] // التوكن مطلوب لأي عملية في هذا الـ Controller
    public class AddressController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddressController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // *********** GET: api/Addresses ***********
        [HttpGet]
        public IActionResult GetAddresses()
        {
            var addresses = _unitOfWork.Addresses.GetAll();
            return Ok(addresses);
        }

        // *********** GET: api/Addresses/5 ***********
        [HttpGet("{id}")]
        public IActionResult GetAddress(int id)
        {
            var address = _unitOfWork.Addresses.GetById(id);
            if (address == null)
            {
                return NotFound(new { message = "Address not found." });
            }
            return Ok(address);
        }

        // *********** POST: api/Addresses ***********
        [HttpPost]
        public IActionResult CreateAddress([FromBody] ShippingAddress shippingAddress)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // استلام التوكن والـ userId
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized(new { message = "User not authenticated." });

            int userId = int.Parse(userIdClaim.Value);

            // تحويل DTO إلى Model Address
            var address = new Address
            {
                UserId = userId, // تعيين userId للمستخدم الذي أنشأ العنوان
                Street = shippingAddress.Street,
                City = shippingAddress.City,
                State = shippingAddress.State,
                ZipCode = shippingAddress.ZipCode,
                Country = shippingAddress.Country
            };

            _unitOfWork.Addresses.Add(address);
            _unitOfWork.Save();

            return CreatedAtAction(nameof(GetAddress), new { id = address.AddressId }, address);
        }

        // *********** PUT: api/Addresses/5 ***********
        [HttpPut("{id}")]
        public IActionResult UpdateAddress(int id, [FromBody] ShippingAddress shippingAddress)
        {
            var existingAddress = _unitOfWork.Addresses.GetById(id);
            if (existingAddress == null)
                return NotFound(new { message = "Address not found." });

            // استلام التوكن والـ userId
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized(new { message = "User not authenticated." });

            int userId = int.Parse(userIdClaim.Value);

            // التحقق من أن الـ address ينتمي للمستخدم
            if (existingAddress.UserId != userId)
                return Unauthorized(new { message = "You are not authorized to update this address." });

            // تحديث العنوان
            existingAddress.Street = shippingAddress.Street;
            existingAddress.City = shippingAddress.City;
            existingAddress.State = shippingAddress.State;
            existingAddress.ZipCode = shippingAddress.ZipCode;
            existingAddress.Country = shippingAddress.Country;

            _unitOfWork.Addresses.Update(existingAddress);
            _unitOfWork.Save();

            return NoContent();
        }

        // *********** DELETE: api/Addresses/5 ***********
        [HttpDelete("{id}")]
        public IActionResult DeleteAddress(int id)
        {
            var address = _unitOfWork.Addresses.GetById(id);
            if (address == null)
                return NotFound(new { message = "Address not found." });

            // استلام التوكن والـ userId
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized(new { message = "User not authenticated." });

            int userId = int.Parse(userIdClaim.Value);

            // التحقق من أن الـ address ينتمي للمستخدم
            if (address.UserId != userId)
                return Unauthorized(new { message = "You are not authorized to delete this address." });

            try
            {
                _unitOfWork.Addresses.Delete(address);
                _unitOfWork.Save();

                return Ok(new { message = "Address deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error deleting address.", error = ex.Message });
            }
        }
    }
}
