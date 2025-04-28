using RahtakApi.Entities.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

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
    public DateTime ScheduledDateTime { get; set; }

    [Required]
    [ForeignKey("BookingStatus")]
    public int BookingStatusId { get; set; }
    public BookingStatus? BookingStatus { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalBookingPrice { get; set; }

    // إضافة الحجز إلى تفاصيل الحجز
    [JsonIgnore]  // تجنب التسلسل المترابط
    public ICollection<BookingDetails> BookingDetails { get; set; } = new List<BookingDetails>();

    // حساب TotalBookingPrice بناءً على التفاصيل
    public void CalculateTotalBookingPrice()
    {
        TotalBookingPrice = BookingDetails.Sum(bd => bd.Price * bd.Quantity);
    }
}
