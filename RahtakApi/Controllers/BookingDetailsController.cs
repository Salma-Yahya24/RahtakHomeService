using Interfaces;
using Microsoft.AspNetCore.Mvc;
using RahtakApi.Entities.Models;
using Microsoft.EntityFrameworkCore; // عشان Include

namespace RahtakApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingDetailsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public BookingDetailsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult GetBookingDetails()
        {
            var bookingDetails = _unitOfWork.BookingDetails
                .GetAll()
                .Include(bd => bd.Booking)
                .Include(bd => bd.SubService)
                .Include(bd => bd.ServiceProvider)
                .ToList();

            return Ok(bookingDetails);
        }

        [HttpGet("{id}")]
        public IActionResult GetBookingDetail(int id)
        {
            var bookingDetail = _unitOfWork.BookingDetails
                .GetByIdWithIncludes(id, bd => bd.Booking, bd => bd.SubService, bd => bd.ServiceProvider);

            if (bookingDetail == null)
            {
                return NotFound();
            }

            return Ok(bookingDetail);
        }

        [HttpPost]
        public IActionResult CreateBookingDetail([FromBody] BookingDetails bookingDetail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _unitOfWork.BookingDetails.Add(bookingDetail);
            _unitOfWork.Save();

            return CreatedAtAction("GetBookingDetail", new { id = bookingDetail.Id }, bookingDetail);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateBookingDetail(int id, [FromBody] BookingDetails bookingDetail)
        {
            if (id != bookingDetail.Id)
            {
                return BadRequest();
            }

            var existingBookingDetail = _unitOfWork.BookingDetails.GetById(id);
            if (existingBookingDetail == null)
            {
                return NotFound();
            }

            existingBookingDetail.BookingId = bookingDetail.BookingId;
            existingBookingDetail.SubServiceId = bookingDetail.SubServiceId;
            existingBookingDetail.ServiceProviderId = bookingDetail.ServiceProviderId;
            existingBookingDetail.Price = bookingDetail.Price;
            existingBookingDetail.Quantity = bookingDetail.Quantity;

            _unitOfWork.BookingDetails.Update(existingBookingDetail);
            _unitOfWork.Save();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBookingDetail(int id)
        {
            var bookingDetail = _unitOfWork.BookingDetails.GetById(id);
            if (bookingDetail == null)
            {
                return NotFound();
            }

            _unitOfWork.BookingDetails.Delete(bookingDetail);
            _unitOfWork.Save();

            return NoContent();
        }
    }
}
