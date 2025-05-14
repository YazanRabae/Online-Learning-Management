using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Service.DTOs.Students
{
    using System.ComponentModel.DataAnnotations;

    public class CreateUserDto : IValidatableObject
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(50, ErrorMessage = "Name can't be longer than 50 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        [StringLength(100, ErrorMessage = "Email can't be longer than 100 characters.")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@(gmail|yahoo|outlook|hotmail)\.com$", ErrorMessage = "Email must be a valid address and from @gmail.com, @yahoo.com, @outlook.com, or @hotmail.com.")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 50 characters.")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string UserId { get; set; }
        public string RoleName { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Only enforce password rules if creating a new user (Id == 0)
            if (Id == 0)
            {
                if (string.IsNullOrWhiteSpace(Password))
                {
                    yield return new ValidationResult("Password is required.", new[] { nameof(Password) });
                }
                if (string.IsNullOrWhiteSpace(ConfirmPassword))
                {
                    yield return new ValidationResult("Confirmation password is required.", new[] { nameof(ConfirmPassword) });
                }
                else if (Password != ConfirmPassword)
                {
                    yield return new ValidationResult("Password and confirmation password do not match.", new[] { nameof(ConfirmPassword) });
                }
            }
        }
    }

}
