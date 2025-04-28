using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RahtakApi.Entities.DTOs;

public class UserRegister
{
    [Required]
    [StringLength(50)]
    public string UserName { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(100)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[A-Za-z\d@$!%*?&]{8,}$",
        ErrorMessage = "Password must be at least 8 characters long, contain uppercase, lowercase, and numbers.")]
    public string Password { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string rePassword { get; set; } = string.Empty;

    [StringLength(15)]
    public string? TelephoneNumber { get; set; }

    public DateTime? DateOfBirth { get; set; }

    [Required]
    [StringLength(10)]
    public string Gender { get; set; } = string.Empty;
}