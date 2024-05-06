using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using WEBTemplate.Models;
using WEBTemplate.Repository;

namespace WEBTemplate.Pages
{
    public class LoginModel : PageModel
    {
        private readonly ILoginRepository _loginRepository;
        private readonly IHttpContextAccessor _httpContext;

        public IEnumerable<UserLoginModel> userLogin { get; set; }

        public LoginModel(ILoginRepository loginRepository, IHttpContextAccessor httpContext)
        {
            _loginRepository = loginRepository;
            _httpContext = httpContext;
            userLogin = new List<UserLoginModel>();

        }

        public void OnGet()
        {

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
