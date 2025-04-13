﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RahtakApi.Entities.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string userEmail, string userPassword, string to, string subject, string body);
    }

}

