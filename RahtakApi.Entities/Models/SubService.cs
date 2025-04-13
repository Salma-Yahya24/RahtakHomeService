using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RahtakApi.Entities.Models;

public class SubService
{
    [Key]
    public int SubServiceId { get; set; }

    [Required]
    [StringLength(100)]
    public string SubServiceName { get; set; } = string.Empty;

    [StringLength(500)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    public bool Enabled { get; set; } = true;

    [ForeignKey("ServiceGroups")]
    public int ServiceGroupId { get; set; }
    public ServiceGroups ServiceGroups { get; set; }

    [ForeignKey("ServiceProvider")]
    public int ServiceProviderId { get; set; }
    public ServiceProviders ServiceProvider { get; set; }
}
