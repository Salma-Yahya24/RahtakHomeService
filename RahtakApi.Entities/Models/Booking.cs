using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RahtakApi.Entities.Models;

public class Booking
{
    [Key]
    public int BookingId { get; set; }

    [Required]
    [ForeignKey("Users")]
    public int UserId { get; set; }
    public Users? User { get; set; }

    [Required]
    public DateTime BookingDate { get; set; } = DateTime.Now;

    [Required]
    [ForeignKey("BookingStatus")]
    public int BookingStatusId { get; set; }
    public BookingStatus? BookingStatus { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalBookingPrice { get; set; }

    // ✅ Adding Navigation Property for BookingDetails
    public ICollection<BookingDetails> BookingDetails { get; set; } = new List<BookingDetails>();
}
