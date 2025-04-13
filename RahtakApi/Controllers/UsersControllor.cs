using Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RahtakApi.Entities.DTOs;
using RahtakApi.Entities.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;

namespace RahtakApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersControllor : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public UsersControllor(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }
        [HttpGet]
        public IActionResult GetUsers()
        {
            var users = _unitOfWork.Users.GetAll()
                .Select(user => new
                {
                    user.Gender,
                    user.FullName,
                    user.Email,
                    user.Role
                });

            return Ok(users);
        }


        [HttpGet("{id}")]
        public IActionResult GetUser(int id)
        {
            var user = _unitOfWork.Users.GetById(id);

            if (user == null)
                return NotFound(new { message = "User not found." });

            var userDto = new
            {
                user.Gender,
                user.FullName,
                user.Email,
                user.Role
            };

            return Ok(userDto);
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] UserRegister user)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid input data.", errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)) });

            // تحقق مما إذا كان البريد الإلكتروني مسجلاً من قبل
            if (_unitOfWork.Users.GetAll().Any(u => u.Email == user.Email))
                return Conflict(new { message = "This email is already registered." });

            var newUser = new Users()
            {
                UserName = user.UserName,
                FullName = user.FullName,
                Email = user.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(user.Password), // تشفير كلمة المرور
                Role = user.Role,
                TelephoneNumber = user.TelephoneNumber,
                DateOfBirth = user.DateOfBirth,
                Gender = user.Gender,
            };

            _unitOfWork.Users.Add(newUser);
            _unitOfWork.Save();

            return Ok(new { message = "User registered successfully!" });
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLogin user)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var existingUser = _unitOfWork.Users.GetAll().FirstOrDefault(u => u.Email == user.Email);
            if (existingUser == null || !BCrypt.Net.BCrypt.Verify(user.Password, existingUser.Password))
                return Unauthorized(new { message = "Invalid email or password" });

            return Ok(new
            {
                message = "Login successful",
                token = GenerateJwtToken(existingUser),
                
            });
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] UpdateUserDto userDto)
        {
            if (userDto == null || string.IsNullOrWhiteSpace(userDto.UserName) || string.IsNullOrWhiteSpace(userDto.FullName))
                return BadRequest(new { message = "Invalid data. Username and Full Name are required." });

            var existingUser = _unitOfWork.Users.GetById(id);
            if (existingUser == null) return NotFound(new { message = "User not found." });

            // تحديث فقط UserName و FullName
            existingUser.UserName = userDto.UserName;
            existingUser.FullName = userDto.FullName;

            _unitOfWork.Users.Update(existingUser);
            _unitOfWork.Save();

            return Ok(new { message = "User updated successfully." });
        }



        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var user = _unitOfWork.Users.GetById(id);
            if (user == null) return NotFound(new { message = "User not found." });

            _unitOfWork.Users.Delete(user);
            _unitOfWork.Save();

            return Ok(new { message = "User deleted successfully." });
        }

        private string GenerateJwtToken(Users user)
        {
            var key = Encoding.UTF8.GetBytes(_configuration["JwtSettings:Secret"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role),
                }),
                Expires = DateTime.UtcNow.AddMinutes(int.Parse(_configuration["JwtSettings:ExpiryMinutes"])),
                Issuer = _configuration["JwtSettings:Issuer"],
                Audience = _configuration["JwtSettings:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
        }
    }
}
