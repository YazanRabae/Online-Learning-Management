using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Repository.Repositories.Shared
{
    public interface ISharedRepository
    {
        Task<(int, int, int, List<string>, List<int>)> GetCounts(string role, string userId);
    }
}
