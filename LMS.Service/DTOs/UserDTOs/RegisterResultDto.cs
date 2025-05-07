using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Service.DTOs.UserDTOs
{
    public class RegisterResultDto
    {
        public string MessageError { get; set; }
        public string UserId { get; set; }
        public bool IsSuccess { get; set; }
    }
}
