using APPTemplate.Models;
using APPTemplate.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace APPTemplate.Pages
{
    public class LoginModel : PageModel
    {
        private readonly ILoginRepository _loginRepository;
        private readonly IHttpContextAccessor _httpContext;

        public IEnumerable<UserLoginModel> userLogin { get; set; }

        [BindProperty]
        public string pagetitle { get; set; }
        public string webversion { get; set; } = APPCommon.RevisionHistory.RevisionHistory.appVersion.ToString("N2");

        public LoginModel(ILoginRepository loginRepository, IHttpContextAccessor httpContext)
        {
            _loginRepository = loginRepository;
            _httpContext = httpContext;
            userLogin = new List<UserLoginModel>();
        }

        public void OnGet()
        {
            _httpContext.HttpContext.Session.SetString("MyTitle", "LAPTOP PASS");

            pagetitle = _httpContext.HttpContext.Session.GetString("MyTitle");
        }

        public async Task<IActionResult> OnGetUserLoginAsync(string parapass)
        {
            userLogin = await _loginRepository.GetLogin(parapass);

            var result = string.Empty;

            if (!userLogin.Any())
            {
                result = JsonSerializer.Serialize(new { Success = false });
            }
            else
            {

                _httpContext.HttpContext.Session.SetString("MyUser", userLogin.Select(x => x.username).FirstOrDefault());
                _httpContext.HttpContext.Session.SetString("MyEmpNo", userLogin.Select(x => x.empno).FirstOrDefault());
                _httpContext.HttpContext.Session.SetInt32("MyAccess", userLogin.Select(x => x.appaccess).FirstOrDefault());


                result = JsonSerializer.Serialize(new { Success = true });
            }

            return new JsonResult(result);
        }

        public async Task<IActionResult> OnGetSuccessLoginAsync()
        {
            return RedirectToPage("/Account/Role");
        }
    }
}