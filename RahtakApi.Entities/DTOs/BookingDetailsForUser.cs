using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RahtakApi.Entities.DTOs
{
    public class BookingDetailsForUser
    {
        public int SubServiceId { get; set; }
        public string SubServiceName { get; set; } = string.Empty;  // حط الاسم هنا أو البيانات اللي انت محتاجها
        public string ServiceProviderName { get; set; } = string.Empty;  // اسم مزود الخدمة
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
