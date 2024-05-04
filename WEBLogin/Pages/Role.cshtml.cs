using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Text.Json;
using WEBLogin.Models;
using WEBLogin.Repository;
using static WEBLogin.Models.LaptopPassModel;

namespace WEBLogin.Pages.Account
{
    public class RoleModel : PageModel
    {
        private readonly ILaptopPassRepository _laptopPassRepository;

        public IEnumerable<LaptopPassRoleModel> roleList { get; set; }

        public RoleModel(ILaptopPassRepository laptopPassRepository)
        {
            _laptopPassRepository = laptopPassRepository;
            roleList = new List<LaptopPassRoleModel>();
        }





        public async Task OnGetAsync()
        {
            roleList = (await _laptopPassRepository.GetRole());



            //var resultlist = new List<string>();

            //if (!loginList.Any())
            //{
            //    resultlist.Add("");
            //}
            //else
            //{
            //    string myuser = JsonSerializer.Serialize(loginList);

            //    TempData["CurrentUser"] = myuser;

            //    resultlist.Add("loginsucess");
            //}


            //return new JsonResult(resultlist);



            //if (TempData["CurrentUser"] == null)
            //{
            //    return RedirectToPage("/Account/Login");
            //}
            //else
            //{
            //    return Page();
            //}

        }
    }
}
