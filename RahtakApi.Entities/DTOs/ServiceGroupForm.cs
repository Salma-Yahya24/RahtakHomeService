using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RahtakApi.Entities.DTOs
{
    public class ServiceGroupForm
    {
        [Required]
        public string ServiceGroupName { get; set; } = string.Empty;

        public string? Description { get; set; }

        public IFormFile? Image { get; set; }
        public string? ImageUrl
        {
            get;
        }
        }
}
