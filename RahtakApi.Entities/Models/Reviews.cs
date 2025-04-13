using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RahtakApi.Entities.Models;

public class Reviews
{
    [Key]
    public int ReviewId { get; set; }

    [Required]
    [ForeignKey("Users")]
    public int UserId { get; set; }
    public Users? User { get; set; }

    [Required]
    [ForeignKey("ServiceProviders")]
    public int ServiceProviderId { get; set; }
    public ServiceProviders? ServiceProvider { get; set; }

    [Required]
    [Range(1, 5)]
    public int Rating { get; set; }

    [StringLength(500)]
    public string? Comment { get; set; }
}
