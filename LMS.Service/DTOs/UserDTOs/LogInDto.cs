using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Service.DTOs.UserDTOs
{
    public class LogInDto
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        [StringLength(100, ErrorMessage = "Email can't be longer than 100 characters.")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@(gmail|yahoo|outlook|hotmail)\.com$", ErrorMessage = "Email must be a valid address and from @gmail.com, @yahoo.com, @outlook.com, or @hotmail.com.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 50 characters.")]
        public string Password { get; set; }

        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
    }
}
