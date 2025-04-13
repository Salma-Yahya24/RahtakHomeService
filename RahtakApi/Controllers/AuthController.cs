using Microsoft.AspNetCore.Mvc;
using RahtakApi.Entities.DTOs;
using RahtakApi.Entities.Interfaces;
using System;
using System.Threading.Tasks;

namespace RahtakApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;

        public AuthController(IUnitOfWork unitOfWork, IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _emailService = emailService;
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest model)
        {
            if (string.IsNullOrWhiteSpace(model.Email))
                return BadRequest(new { message = "Email is required." });

            var user = await _unitOfWork.Users.GetUserByEmailAsync(model.Email);
            if (user == null)
                return NotFound(new { message = "User not found." });

            var resetCode = new Random().Next(100000, 999999).ToString();

            user.ResetCode = resetCode;
            user.ResetCodeExpiration = DateTime.UtcNow.AddMinutes(15);
            _unitOfWork.Users.Update(user);
            await _unitOfWork.SaveAsync();

            bool emailSent = await _emailService.SendEmailAsync(
                "rahtakhomeservice@gmail.com",
                "yrtc gzrw iufp fump",
                model.Email,
                "Reset Password Code",
                $"Your reset code is: {resetCode}"
            );

            if (!emailSent)
                return StatusCode(500, new { message = "Failed to send email. Try again later." });

            return Ok(new { message = "Reset code has been sent to your email." });
        }

        [HttpPost("verify-code")]
        public async Task<IActionResult> VerifyCode([FromBody] VerifyCodeRequest model)
        {
            if (string.IsNullOrWhiteSpace(model.ResetCode))
                return BadRequest(new { message = "Reset code is required." });

            var user = await _unitOfWork.Users.GetUserByResetCodeAsync(model.ResetCode);
            if (user == null)
                return BadRequest(new { message = "Invalid reset code." });

            if (user.ResetCodeExpiration < DateTime.UtcNow)
                return BadRequest(new { message = "Reset code has expired." });

            return Ok(new { message = "Reset code is valid." });
        }
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest model)
        {
            if (string.IsNullOrWhiteSpace(model.ResetCode) || string.IsNullOrWhiteSpace(model.NewPassword))
                return BadRequest(new { message = "Reset code and new password are required." });

            var user = await _unitOfWork.Users.GetUserByResetCodeAsync(model.ResetCode);
            if (user == null)
                return BadRequest(new { message = "Invalid reset code." });

            if (user.ResetCodeExpiration < DateTime.UtcNow)
                return BadRequest(new { message = "Reset code has expired." });

            // ✅ تشفير كلمة المرور الجديدة
            user.Password = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);

            user.ResetCode = null;
            user.ResetCodeExpiration = null;

            _unitOfWork.Users.Update(user);
            await _unitOfWork.SaveAsync();

            return Ok(new { message = "Password has been reset successfully." });
        }

    }
}
