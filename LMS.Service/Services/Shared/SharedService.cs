using LMS.Repository.Repositories.Shared;

namespace LMS.Service.Services.Shared
{
    public class SharedService : ISharedService
    {
        private readonly ISharedRepository _sharedRepository;

        public SharedService(ISharedRepository sharedRepository)
        {
            _sharedRepository = sharedRepository;
        }
        public async Task<(int, int, int, List<string>, List<int>)> GetCounts(string role, string userId)
        {
            return await _sharedRepository.GetCounts(role, userId);
        }
    }
}
