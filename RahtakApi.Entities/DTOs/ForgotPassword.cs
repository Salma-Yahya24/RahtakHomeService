using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RahtakApi.Entities.DTOs;

public class ForgotPasswordRequest
{
    public string Email { get; set; }
}

public class VerifyCodeRequest
{
    public string ResetCode { get; set; }
}

public class ResetPasswordRequest
{
    public string ResetCode { get; set; }
    public string NewPassword { get; set; }
}


