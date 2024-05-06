using Microsoft.AspNetCore.Mvc.RazorPages;
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

        public void OnGet()
        {
            //Check if Login
            //if (TempData.Peek("CurrentUser")==null)
            //{
            //    return RedirectToPage("/Login");
            //}
            //else
            //{
            //    return Page();
            //}
        }
    }
}