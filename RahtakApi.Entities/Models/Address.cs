using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RahtakApi.Entities.Models;

public class Address
{
    [Key]
    public int AddressId { get; set; }

    [ForeignKey("Users")]
    public int? UserId { get; set; }
    public Users? User { get; set; }

    [ForeignKey("ServiceProviders")]
    public int? ServiceProviderId { get; set; }
    public ServiceProviders? ServiceProvider { get; set; }

    [StringLength(200)]
    public string? Street { get; set; }

    [StringLength(100)]
    public string? City { get; set; }

    [StringLength(100)]
    public string? State { get; set; }

    [StringLength(20)]
    public string? ZipCode { get; set; }

    [StringLength(100)]
    public string? Country { get; set; }
}
