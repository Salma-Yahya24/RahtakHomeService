using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RahtakApi.Entities.DTOs
{
    public class BookSubService
    {
        
        public int SubServiceId { get; set; }
        // ✅ إضافة حقل جديد للتاريخ والوقت
        [Required(ErrorMessage = "Scheduled date and time are required.")]
        public DateTime ScheduledDateTime { get; set; }
    }

}
