using APPCommon.Class;
using ATSSFG.Models;
using ATSSFG.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualBasic.ApplicationServices;
using System.Security.Claims;


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

        public async Task<IActionResult> OnGetAsync()
        {

            Console.WriteLine(User.Identity.IsAuthenticated ? "Authenticated!" : "Not Authenticated. :(");
            Console.WriteLine(User.FindFirstValue("FullName"));
            Console.WriteLine(User.FindFirstValue("Password"));
            Console.WriteLine(User.FindFirstValue("EmpNo"));

            //Check if user exists in Database
            //string userName = User.FindFirstValue("FullName");
            string userName = "Michelle Adrales";
            //Remove  this when publishing
            //z = "Michelle Adrales";
            //y = "MENU";
            //x = "ATS";
            //w = "Cost Engineering";



            if (string.IsNullOrEmpty(userName))
            {
                return Redirect(PIMESSettings.lnkLogin);
            }
            else
            {
                UsersInfoModel user = await CheckUser(userName);
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