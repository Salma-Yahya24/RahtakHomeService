using RahtakApi.Entities.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class ServiceProviders
{
    [Key]
    public int ServiceProviderId { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(15)]
    public string Telephone { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Email { get; set; } = string.Empty;

    public bool Enabled { get; set; } = true;

    [ForeignKey("ServiceProviderType")]
    public int ServiceProviderTypeId { get; set; }

    public ServiceProviderType? ServiceProviderType { get; set; }

}
