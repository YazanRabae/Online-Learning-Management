using System.ComponentModel.DataAnnotations;
//using System.Web.Mvc;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Service.DTOs.UserDTOs;

public class RegisterDto
{
    [Required(ErrorMessage = "Name is required.")]
    [StringLength(50, ErrorMessage = "Name can't be longer than 50 characters.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address format.")]
    [StringLength(100, ErrorMessage = "Email can't be longer than 100 characters.")]
    [RegularExpression(@"^[a-zA-Z0-9._%+-]+@(gmail|yahoo|outlook|hotmail)\.com$", ErrorMessage = "Email must be a valid address and from @gmail.com, @yahoo.com, @outlook.com, or @hotmail.com.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [DataType(DataType.Password)]
    [StringLength(50, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 50 characters.")]
    public string Password { get; set; }

    [Required(ErrorMessage = "Confirmation password is required.")]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    [Compare("Password", ErrorMessage = "Password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; }
}

