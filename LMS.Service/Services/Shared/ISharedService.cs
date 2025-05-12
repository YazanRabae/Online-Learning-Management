using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Service.Services.Shared
{
    public interface ISharedService
    {
        Task<(int, int, int, List<string>, List<int>)> GetCounts(string role, string userId);
    }
}
