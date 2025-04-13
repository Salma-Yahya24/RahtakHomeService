using Interfaces;
using Microsoft.AspNetCore.Mvc;
using RahtakApi.Entities.Models;

namespace RahtakApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public BookingController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // *********** GET: api/Bookings ***********
        [HttpGet]
        public IActionResult GetBookings()
        {
            var bookings = _unitOfWork.Bookings.GetAll();
            return Ok(bookings);
        }

        // *********** GET: api/Bookings/5 ***********
        [HttpGet("{id}")]
        public IActionResult GetBooking(int id)
        {
            var booking = _unitOfWork.Bookings.GetById(id);

            if (booking == null)
            {
                return NotFound();
            }

            return Ok(booking);
        }

        // *********** POST: api/Bookings ***********
        [HttpPost]
        public IActionResult CreateBooking([FromBody] Booking booking)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _unitOfWork.Bookings.Add(booking);
            _unitOfWork.Save();

            return CreatedAtAction("GetBooking", new { id = booking.BookingId }, booking);
        }

        // *********** PUT: api/Bookings/5 ***********
        [HttpPut("{id}")]
        public IActionResult UpdateBooking(int id, [FromBody] Booking booking)
        {
            if (id != booking.BookingId)
            {
                return BadRequest();
            }

            var existingBooking = _unitOfWork.Bookings.GetById(id);
            if (existingBooking == null)
            {
                return NotFound();
            }

            existingBooking.UserId = booking.UserId;
            existingBooking.BookingDate = booking.BookingDate;
            existingBooking.BookingStatusId = booking.BookingStatusId;
            existingBooking.TotalBookingPrice = booking.TotalBookingPrice;

            _unitOfWork.Bookings.Update(existingBooking);
            _unitOfWork.Save();

            return NoContent();
        }

        // *********** DELETE: api/Bookings/5 ***********
        [HttpDelete("{id}")]
        public IActionResult DeleteBooking(int id)
        {
            var booking = _unitOfWork.Bookings.GetById(id);
            if (booking == null)
            {
                return NotFound();
            }

            _unitOfWork.Bookings.Delete(booking);
            _unitOfWork.Save();

            return NoContent();
        }
    }
}
