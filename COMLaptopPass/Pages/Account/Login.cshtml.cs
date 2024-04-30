using COMLaptopPass.Models;
using COMLaptopPass.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace COMLaptopPass.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly ILaptopPassRepository laptopPassRepository;


        public IEnumerable<LaptopPassRoleModel> userList { get; set; } = new List<LaptopPassRoleModel>();


        [BindProperty]


        public LaptopPassParameters param { get; set; } = new LaptopPassParameters();
        public SelectList buList { get; set; }
        public SelectList roleList { get; set; }



        public LoginModel(ILaptopPassRepository _laptopPassRepository)
        {
            laptopPassRepository = _laptopPassRepository;
        }
        public void OnGet()
        {
            param.password = "";

            // userList = (await laptopPassRepository.GetLogin()).ToList();


            // roleList = new SelectList(userList.Select(x => x.username));


        }



        public async Task<IActionResult> OnPostLoginAsync()
        {

            userList = (await laptopPassRepository.GetLogin()).ToList();

            TempData["tempUser"] = JsonConvert.SerializeObject(userList);


            var currentUser = userList.Where(x => x.password == param.password).FirstOrDefault();

            if (currentUser == null || string.IsNullOrEmpty(currentUser.username))
            {

                //param.password = "";
                // return Page();

            }
            else
            {

                //Insert to Claims Identity


                buList = new SelectList(userList.Where(x => x.userid == currentUser.userid).Select(x => x.bucode));

                // return Page();

                // return RedirectToPage("/LaptopPass/Index");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostRoleListAsync()
        {

            userList = JsonConvert.DeserializeObject<IEnumerable<LaptopPassRoleModel>>(TempData["tempUser"].ToString());


           LaptopPassRoleModel currentUser = userList.Where(x => x.password == param.password).FirstOrDefault();

            if (currentUser == null || string.IsNullOrEmpty(currentUser.username))
            {

                param.password = "";
                // return Page();

            }
            else
            {

                //Insert to Claims Identity

                List<string> varlist = new List<string>();

                foreach (LaptopPassRoleModel item in userList.Where(x => x.userid == currentUser.userid && x.bucode == param.bucode))
                {
                    if (item.requestor > 0)
                    {
                        varlist.Add("REQUESTOR");
                    }
                    if (item.approver > 0)
                    {
                        varlist.Add("APPROVER");
                    }
                    if (item.noter > 0)
                    {
                        varlist.Add("NOTER");
                    }
                }

                roleList = new SelectList(varlist);


                // List<string> varlist = new List<string>();

               // var datavar = new SelectList(userList.Where(x => x.userid == currentUser.userid && x.bucode == param.bucode).Select(x => x.username));


         

                // return Page();

                // return RedirectToPage("/LaptopPass/Index");
            }

            return Page();
        }
    }
}
