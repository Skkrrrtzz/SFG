
using APPCommon.Class;
using APPTemplate.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace APPTemplate.Pages
{
    public class MenuModel : PageModel
    {

        #region Declaration

        private readonly IHttpContextAccessor _httpContext;
        private readonly ILaptopPassRepository _laptopPassRepository;


        #endregion Declaration

        #region Binding

        [BindProperty]
        public string pagetitle { get; set; }

        public string webversion { get; set; } = APPCommon.RevisionHistory.RevisionHistory.appVersion.ToString("N2");

        #endregion Binding

        #region Constructor

        public MenuModel(IHttpContextAccessor httpContext, ILaptopPassRepository laptopPassRepository)
        {
            _httpContext = httpContext;
            _laptopPassRepository = laptopPassRepository;
        }

        #endregion Constructor

        #region Function
    

        #endregion Function

        #region Get

        public async Task<IActionResult> OnGetAsync()
        {

            if (string.IsNullOrEmpty(_httpContext.HttpContext.Session.GetString("MyUser")))
            {
                return Redirect(PIMESSettings.lnkLogin);
            }
            else
            {
                return Page();
            }

        }



        #endregion Get
    }
}