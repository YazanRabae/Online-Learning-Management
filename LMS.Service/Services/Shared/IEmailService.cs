using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Service.Services.Shared
{
    public interface IEmailService
    {
        Task SendEmailAsync(string receiverName, string receiverMail, string subject, string body);
    }
}
