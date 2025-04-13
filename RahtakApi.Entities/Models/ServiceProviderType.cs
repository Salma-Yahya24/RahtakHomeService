using System;
using System.ComponentModel.DataAnnotations;

namespace RahtakApi.Entities.Models;

public class ServiceProviderType
{
    [Key]
    public int ServiceProviderTypeId { get; set; }

    [Required]
    [StringLength(100)]
    public string Type { get; set; } = string.Empty;
}
