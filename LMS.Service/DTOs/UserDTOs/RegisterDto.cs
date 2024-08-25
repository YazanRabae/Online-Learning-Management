using System.ComponentModel.DataAnnotations;
//using System.Web.Mvc;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Service.DTOs.UserDTOs;

public class RegisterDto
{
    [Required]
    public string Name { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    [Compare("Password",
    ErrorMessage = "Password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; }
}

