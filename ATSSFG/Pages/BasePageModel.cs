using ATSSFG.Models;
using ATSSFG.Repository;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ATSSFG.Pages
{
    public class BasePageModel : PageModel
    {
        protected readonly ISessionService _sessionService;
        protected readonly IUserRepository _userRepository;

        public BasePageModel(ISessionService sessionService, IUserRepository userRepository)
        {
            _sessionService = sessionService;
            _userRepository = userRepository;
        }

        public async Task<UsersInfoModel> CheckUser(string name)
        {
            return await _userRepository.CheckUser(name);
        }

    }

}
