using Interfaces;
using Microsoft.AspNetCore.Mvc;
using RahtakApi.Entities.Models;

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

        // *********** GET: api/BookingDetails ***********
        [HttpGet]
        public IActionResult GetBookingDetails()
        {
            var bookingDetails = _unitOfWork.BookingDetails.GetAll();
            return Ok(bookingDetails);
        }

        // *********** GET: api/BookingDetails/5 ***********
        [HttpGet("{id}")]
        public IActionResult GetBookingDetail(int id)
        {
            var bookingDetail = _unitOfWork.BookingDetails.GetById(id);

            if (bookingDetail == null)
            {
                return NotFound();
            }

            return Ok(bookingDetail);
        }

        // *********** POST: api/BookingDetails ***********
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

        // *********** PUT: api/BookingDetails/5 ***********
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

        // *********** DELETE: api/BookingDetails/5 ***********
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
