using RahtakApi.Entities.DTOs;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RahtakApi.Entities.Models;

public class Payments
{
    [Key]
    public int PaymentId { get; set; }

    [Required]
    [ForeignKey("Booking")]
    public int BookingId { get; set; }
    public Booking Booking { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime PaymentDate { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }

    [Required]
    [StringLength(50)]
    public string PaymentStatus { get; set; } = string.Empty;

    [Required]
    [ForeignKey("PaymentMethod")]
    public int PaymentMethodId { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    
}
