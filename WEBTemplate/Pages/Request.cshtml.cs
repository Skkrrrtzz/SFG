using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using WEBTemplate.Models;
using WEBTemplate.Repository;

namespace WEBTemplate.Pages
{
    public class RequestModel : PageModel
    {
        private readonly ILaptopPassRepository _laptopPassRepository;

        public RequestModel(ILaptopPassRepository laptopPassRepository)
        {
            _laptopPassRepository = laptopPassRepository;
        }

        public async Task<IActionResult> OnGetAsync()
        {

            //Check if Login
            if (TempData.Peek("CurrentUser")==null)
            {
                return RedirectToPage("/Login");
            }
            else
            {
                return Page();
            }
        }
    }
}
