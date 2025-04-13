using Interfaces;
using Microsoft.AspNetCore.Mvc;
using RahtakApi.Entities.Models;

namespace RahtakApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingStatusesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public BookingStatusesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // *********** GET: api/BookingStatuses ***********
        [HttpGet]
        public IActionResult GetBookingStatuses()
        {
            var bookingStatuses = _unitOfWork.BookingStatuses.GetAll();
            return Ok(bookingStatuses);
        }

        // *********** GET: api/BookingStatuses/5 ***********
        [HttpGet("{id}")]
        public IActionResult GetBookingStatus(int id)
        {
            var bookingStatus = _unitOfWork.BookingStatuses.GetById(id);

            if (bookingStatus == null)
            {
                return NotFound();
            }

            return Ok(bookingStatus);
        }

        // *********** POST: api/BookingStatuses ***********
        [HttpPost]
        public IActionResult CreateBookingStatus([FromBody] BookingStatus bookingStatus)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _unitOfWork.BookingStatuses.Add(bookingStatus);
            _unitOfWork.Save();

            return CreatedAtAction("GetBookingStatus", new { id = bookingStatus.BookingStatusId }, bookingStatus);
        }

        // *********** PUT: api/BookingStatuses/5 ***********
        [HttpPut("{id}")]
        public IActionResult UpdateBookingStatus(int id, [FromBody] BookingStatus bookingStatus)
        {
            if (id != bookingStatus.BookingStatusId)
            {
                return BadRequest();
            }

            var existingBookingStatus = _unitOfWork.BookingStatuses.GetById(id);
            if (existingBookingStatus == null)
            {
                return NotFound();
            }

            existingBookingStatus.StatusName = bookingStatus.StatusName;

            _unitOfWork.BookingStatuses.Update(existingBookingStatus);
            _unitOfWork.Save();

            return NoContent();
        }

        // *********** DELETE: api/BookingStatuses/5 ***********
        [HttpDelete("{id}")]
        public IActionResult DeleteBookingStatus(int id)
        {
            var bookingStatus = _unitOfWork.BookingStatuses.GetById(id);
            if (bookingStatus == null)
            {
                return NotFound();
            }

            _unitOfWork.BookingStatuses.Delete(bookingStatus);
            _unitOfWork.Save();

            return NoContent();
        }
    }
}
