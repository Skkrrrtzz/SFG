using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEBLogin.Models;
using WEBLogin.Repository;

namespace WEBLogin.Pages.Account
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

            loginList = (await _loginRepository.GetLogin()).ToList();

            var templist = loginList
                            .Where(x => x.password == parapass)
                            .Select(x => x.password)
                            .FirstOrDefault();

            var resultlist = new List<string>();

            if (string.IsNullOrEmpty(templist))
            {
                resultlist.Add("");
            }
            else
            {

                resultlist.Add("loginsucess");
            }


            return new JsonResult(resultlist);
        }
    }
}
