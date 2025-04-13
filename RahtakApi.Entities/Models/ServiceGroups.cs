using System;
using System.ComponentModel.DataAnnotations;

namespace RahtakApi.Entities.Models;

public class ServiceGroups
{
    [Key]
    public int ServiceGroupId { get; set; }

    [Required]
    [StringLength(100)]
    public string ServiceGroupName { get; set; } = string.Empty;

    [StringLength(500)]
    public string Description { get; set; } = string.Empty;
}
