using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RahtakApi.Entities.DTOs
{
    public class BookingDataForUser
    {
        public int BookingId { get; set; }
        public DateTime BookingDate { get; set; }
        public string? StatusName { get; set; }
        public decimal TotalBookingPrice { get; set; }
        public DateTime ScheduledDateTime { get; set; }
        public List<BookingDetailsForUser> BookingDetails { get; set; } = new List<BookingDetailsForUser>();
    }
}
