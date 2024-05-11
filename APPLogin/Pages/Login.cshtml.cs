using APPCommon.Class;
using APPLogin.Models;
using APPLogin.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace APPLogin.Pages
{
    public class LoginModel : PageModel
    {
        #region Declaration

        private readonly ILoginRepository _loginRepository;
        private readonly IHttpContextAccessor _httpContext;

        public IEnumerable<UserLoginModel> userLogin { get; set; }

        #endregion Declaration

        #region Binding

        [BindProperty]

        public string loader { get; set; } = PIMESProcedures.randomLoader();
        public string pagetitle { get; set; } = "LOGIN";

        public string webversion { get; set; } = APPCommon.RevisionHistory.RevisionHistory.appVersion.ToString("N2");


        #endregion Binding

        #region Constructor

        public LoginModel(ILoginRepository loginRepository, IHttpContextAccessor httpContext)
        {
            _loginRepository = loginRepository;
            _httpContext = httpContext;
            userLogin = new List<UserLoginModel>();
        }

        #endregion Constructor

        #region Get

        public void OnGet()
        {
            //_httpContext.HttpContext.Session.SetString("MyTitle", "LOGIN");
            //pagetitle = _httpContext.HttpContext.Session.GetString("MyTitle");
        }

        public async Task<IActionResult> OnGetUserLoginAsync(string parapass)
        {
            try
            {
                userLogin = await _loginRepository.GetLogin(parapass);

                var result = string.Empty;


                if (!userLogin.Any())
                {
                    result = JsonSerializer.Serialize(new { Success = false });
                }
                else
                {
                    _httpContext.HttpContext.Session.SetString("MyPassword", userLogin.Select(x => x.password).FirstOrDefault());
                    _httpContext.HttpContext.Session.SetString("MyUser", userLogin.Select(x => x.username).FirstOrDefault());
                    _httpContext.HttpContext.Session.SetString("MyEmpNo", userLogin.Select(x => x.employeeno).FirstOrDefault());

                    result = JsonSerializer.Serialize(new { Success = true });
                }

                return new JsonResult(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { errorMessage = ex.Message });
            }
        }
        #endregion Get
    }
}