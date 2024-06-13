using APPCommon.Class;
using ATSSFG.Models;
using ATSSFG.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace ATSSFG.Pages
{
    public class IndexModel : BasePageModel
    {

        #region Declaration

        private readonly IHttpContextAccessor _httpContext;
        private readonly IUserRepository _userRepository;

        #endregion Declaration

        #region Binding

        #endregion Binding

        #region Constructor

        public IndexModel(ISessionService sessionService, IUserRepository userRepository)
        : base(sessionService, userRepository)
        {
        }

        #endregion Constructor

        #region Function

        #endregion Function

        #region Get

        public async Task<IActionResult> OnGetAsync(string z, string y, string x, string w)
        {
            //Remove  this when publishing
            z = "Michelle Adrales";
            y = "MENU";
            x = "ATS";
            w = "Cost Engineering";



            if (string.IsNullOrEmpty(z))
            {
                return Redirect(PIMESSettings.lnkLogin);
            }
            else
            {
                UsersInfoModel user = await CheckUser(z,w);
                if (user == null) {

                    return Redirect(PIMESSettings.lnkLogin);
                }
                else
                {
                    _sessionService.SetRole("ATS - Sourcing Form Generation", user);
                    return RedirectToPage("Dashboard/Dashboard");
                }
                
            }

        }

        #endregion Get
    }
}