using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RahtakApi.Entities.Models;

public class PaymentMethod
{
    [Key]
    public int PaymentMethodId { get; set; }

    [Required]
    [StringLength(100)]
    public string PaymentTypeDesc { get; set; }
}