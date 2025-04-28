using Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RahtakApi.Entities.DTOs;
using RahtakApi.Entities.Models;
using System.Security.Claims;

namespace RahtakApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // ✅ التوكن مطلوب لأي حاجة في BookingController
    public class BookingController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public BookingController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetBookings()
        {
            var bookings = _unitOfWork.Bookings.GetAll()
                .Include(b => b.User) // تحميل بيانات المستخدم
                .Include(b => b.BookingStatus) // تحميل حالة الحجز
                .Include(b => b.BookingDetails) // تحميل تفاصيل الحجز
                    .ThenInclude(bd => bd.SubService) // تحميل الخدمة الفرعية
                .Include(b => b.BookingDetails)
                    .ThenInclude(bd => bd.ServiceProvider) // تحميل مزود الخدمة
                .ToList();

            return Ok(bookings);
        }
        // ✅ Get Bookings for the Logged-in User
        [HttpGet("MyBookings")]
        public IActionResult GetMyBookings()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized(new { message = "User not authenticated." });

            int userId = int.Parse(userIdClaim.Value);

            var result = _unitOfWork.Bookings
                .FindAll(b => b.UserId == userId)
                .Select(b => new BookingDataForUser
                {
                    BookingId = b.BookingId,
                    BookingDate = b.BookingDate,
                    StatusName = b.BookingStatus.StatusName,
                    ScheduledDateTime = b.ScheduledDateTime,
                    TotalBookingPrice = b.BookingDetails.Sum(bd => bd.Price * bd.Quantity),
                    BookingDetails = b.BookingDetails.Select(bd => new BookingDetailsForUser
                    {
                        SubServiceId = bd.SubServiceId,
                        SubServiceName = bd.SubService.SubServiceName,
                        ServiceProviderName = bd.ServiceProvider.Name,
                        Price = bd.Price,
                        Quantity = bd.Quantity
                    }).ToList()
                })
                .ToList();

            return Ok(result);
        }
        [HttpGet("{id}")]
        public IActionResult GetBooking(int id)
        {
            var booking = _unitOfWork.Bookings.GetById(id);
            if (booking == null) return NotFound(new { message = "Booking not found." });

            return Ok(booking);
        }

        [HttpPost]
        public IActionResult CreateBooking([FromBody] Booking booking)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _unitOfWork.Bookings.Add(booking);
            _unitOfWork.Save();

            return CreatedAtAction(nameof(GetBooking), new { id = booking.BookingId }, booking);
        }

        // ✨ هنا غيرت الراوت: من book-subservice -> BookSubService
        [HttpPost("BookSubService")]
        public IActionResult BookSubService([FromBody] BookSubService dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized(new { message = "User not authenticated." });

            if (dto == null)
                return BadRequest(new { message = "Invalid request data." });

            int userId = int.Parse(userIdClaim.Value);

            var subService = _unitOfWork.SubServices.GetById(dto.SubServiceId);
            if (subService == null)
                return NotFound(new { message = "SubService not found." });

            if (dto.ScheduledDateTime <= DateTime.UtcNow)
                return BadRequest(new { message = "Scheduled date and time must be in the future." });

            var booking = new Booking
            {
                UserId = userId,
                BookingDate = DateTime.UtcNow,
                ScheduledDateTime = dto.ScheduledDateTime,
                BookingStatusId = 1, // Pending
                TotalBookingPrice = subService.Price,
                BookingDetails = new List<BookingDetails>
                {
                    new BookingDetails
                    {
                        SubServiceId = dto.SubServiceId,
                        ServiceProviderId = subService.ServiceProviderId,
                        Price = subService.Price,
                        Quantity = 1
                    }
                }
            };

            try
            {
                _unitOfWork.Bookings.Add(booking);
                _unitOfWork.Save();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new { message = "An error occurred while saving the booking.", error = ex.Message });
            }

            return Ok(new { message = "SubService booked successfully.", bookingId = booking.BookingId });
        }

        [HttpPut("{id}")]
        public IActionResult UpdateBooking(int id, [FromBody] BookingUpdate bookingUpdate)
        {
            var existingBooking = _unitOfWork.Bookings.GetById(id);
            if (existingBooking == null)
                return NotFound(new { message = "Booking not found." });

            // التحقق من أن التاريخ الجديد بعد الوقت الحالي
            if (bookingUpdate.ScheduledDateTime <= DateTime.UtcNow)
            {
                return BadRequest(new { message = "Scheduled date and time must be in the future." });
            }

            // تحديث الجدول الزمني فقط
            existingBooking.ScheduledDateTime = bookingUpdate.ScheduledDateTime;

            _unitOfWork.Bookings.Update(existingBooking);
            _unitOfWork.Save();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBooking(int id)
        {
            var booking = _unitOfWork.Bookings.GetById(id);
            if (booking == null)
                return NotFound(new { message = "Booking not found." });

            try
            {
                _unitOfWork.Bookings.Delete(booking);
                _unitOfWork.Save();

                return Ok(new { message = "Booking deleted successfully." });
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new { message = "Error deleting booking.", error = ex.Message });
            }
        }

    }
}
