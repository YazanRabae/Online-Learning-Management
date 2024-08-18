using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Service.DTOs.UserDTOs;

public class UserClaimsViewModel
{
    public UserClaimsViewModel()
    {
        Claims = new List<UserClaims>();
    }
    public string UserId { get; set; }
    public List<UserClaims> Claims { get; set; }
}
