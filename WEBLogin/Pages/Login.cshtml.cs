using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using WEBLogin.Models;
using WEBLogin.Repository;

namespace WEBLogin.Pages
{
    public class LoginModel : PageModel
    {
        private readonly ILoginRepository _loginRepository;
        public IEnumerable<UserLoginModel> loginList { get; set; }

        public LoginModel(ILoginRepository loginRepository)
        {
            _loginRepository = loginRepository;
            loginList = new List<UserLoginModel>();

        }

        public void OnGet()
        {

        }


        public async Task<IActionResult> OnGetUserLoginAsync(string parapass)
        {
            loginList = await _loginRepository.GetLogin(parapass);

            var resultlist = new List<string>();

            if (!loginList.Any())
            {
                resultlist.Add("");
            }
            else
            {
                string myuser = JsonSerializer.Serialize(loginList);

                TempData["CurrentUser"] = myuser;

                resultlist.Add("loginsucess");
            }


            return new JsonResult(resultlist);
        }




        public async Task<IActionResult> OnGetSuccessLoginAsync()
        {
            return RedirectToPage("/Account/Role");

        }
    }
}
